using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using vts.Core.Shared.Entities.Master;
using vts.Data.Context;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;

namespace vts.Data.Repository.MasterData
{
    public class PollingCentreRepository : BaseRepository<PollingCentre, PollingCentreRef>, IPollingCentreRepository
    {
        private readonly IWardRepository _wardRepository;

        public PollingCentreRepository(ContextConnection contextConnection, IWardRepository wardRepository) : base(contextConnection)
        {
            _wardRepository = wardRepository;
        }

        protected override Func<PollingCentre, List<PollingCentre>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate PollingCentre Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<PollingCentre>, List<PollingCentre>> SearchFunc
        {
            get
            {
                return (searchText, allItems) =>
                {
                    if (string.IsNullOrEmpty(searchText)) return allItems;
                    var st = searchText.ToLower();

                    return
                        allItems.Where(
                            n =>
                                n.Name.ToLower().Contains(st)
                                || n.Code.ToLower().Contains(st))
                            .ToList();
                };
            }
        }

        public Guid Save(PollingCentre entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Polling Centre not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("PollingCentreRepository"))
            {
                PollingCentre pollingCentre = GetById(entity.Id);
                if (pollingCentre == null)
                {
                    entity.Status = EntityStatus.Active;
                    entity.DateCreated = dt;
                }
                entity.DateLastUpdated = dt;
                ctx.UpdateGraph(entity);

                ctx.SaveChanges();
            }

            return entity.Id;
        }

        private void SetStatus(PollingCentre entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("PollingCentreRepository"))
            {
                PollingCentre c = ctx.PollingCentres.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(PollingCentre entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(PollingCentre entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(PollingCentre entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override PollingCentre GetById(Guid id, bool includeDeactivated = false)
        {
            PollingCentre pollingCentre = null;
            using (var ctx = GetVtsContext("PollingCentreRepository"))
            {
                pollingCentre = CtxSetup(ctx.PollingCentres).FirstOrDefault(n => n.Id == id);
            }
            return pollingCentre;
        }

        public override IEnumerable<PollingCentre> GetAll(bool includeDeactivated = false)
        {
            List<PollingCentre> pollingCentre = new List<PollingCentre>();

            using (var ctx = GetVtsContext("PollingCentreRepository"))
            {
                if (includeDeactivated)
                    pollingCentre =
                    CtxSetup(ctx.PollingCentres)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    pollingCentre =
                    CtxSetup(ctx.PollingCentres)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return pollingCentre;
        }

        public ValidationResultInfo Validate(PollingCentre itemToValidate)
        {
            var validation = base.Validate(itemToValidate, GetAll(true).ToList());
            var ward = _wardRepository.GetById(itemToValidate.Ward.Id);
            if (ward == null)
            {
                validation.Results.Add(new ValidationResult("Invalid ward reference."));
            }
            return validation;
        }
    }
}