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
    public class WardRepository : BaseRepository<Ward, WardRef>, IWardRepository
    {
        private readonly IConstituencyRepository _constituencyRepository;

        public WardRepository(ContextConnection contextConnection, IConstituencyRepository constituencyRepository)
             : base(contextConnection)
        {
            _constituencyRepository = constituencyRepository;
        }

        protected override Func<Ward, List<Ward>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate Ward Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<Ward>, List<Ward>> SearchFunc
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

        public Guid Save(Ward entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Ward not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("WardRepository"))
            {
                Ward ward = GetById(entity.Id);
                if (ward == null)
                {
                    entity.Status = EntityStatus.Active;
                    entity.DateCreated = dt;
                }
                entity.DateLastUpdated = dt;
                ctx.UpdateGraph(entity, map => map.OwnedCollection(n => n.WardMcas));

                ctx.SaveChanges();
            }

            return entity.Id;
        }

        private void SetStatus(Ward entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("WardRepository"))
            {
                Ward c = ctx.Wards.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(Ward entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(Ward entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(Ward entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override Ward GetById(Guid id, bool includeDeactivated = false)
        {
            Ward ward = null;
            using (var ctx = GetVtsContext("WardRepository"))
            {
                ward = CtxSetup(ctx.Wards).FirstOrDefault(n => n.Id == id);
            }
            return ward;
        }

        public override IEnumerable<Ward> GetAll(bool includeDeactivated = false)
        {
            List<Ward> ward = new List<Ward>();

            using (var ctx = GetVtsContext("WardRepository"))
            {
                if (includeDeactivated)
                    ward =
                    CtxSetup(ctx.Wards)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    ward =
                    CtxSetup(ctx.Wards)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return ward;
        }

        public ValidationResultInfo Validate(Ward itemToValidate)
        {
            var validation = base.Validate(itemToValidate, GetAll(true).ToList());
            var constituency = _constituencyRepository.GetById(itemToValidate.Constituency.Id);
            if (constituency == null)
            {
                validation.Results.Add(new ValidationResult("Invalid constituency reference."));
            }
            return validation;
        }
    }
}