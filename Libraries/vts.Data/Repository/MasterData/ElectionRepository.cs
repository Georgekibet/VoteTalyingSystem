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
    public class ElectionRepository : BaseRepository<Election, ElectionRef>, IElectionRepository
    {
        public ElectionRepository(ContextConnection contextConnection)
             : base(contextConnection)
        {
        }

        protected override Func<Election, List<Election>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate Election Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<Election>, List<Election>> SearchFunc
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
                                n.Name.ToLower().Contains(st))
                            .ToList();
                };
            }
        }

        public Guid Save(Election entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Election not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("ElectionRepository"))
            {
                Election election = GetById(entity.Id);
                if (election == null)
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

        private void SetStatus(Election entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("ElectionRepository"))
            {
                Election c = ctx.Elections.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(Election entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(Election entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(Election entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override Election GetById(Guid id, bool includeDeactivated = false)
        {
            Election election = null;
            using (var ctx = GetVtsContext("ElectionRepository"))
            {
                election = CtxSetup(ctx.Elections).FirstOrDefault(n => n.Id == id);
            }
            return election;
        }

        public override IEnumerable<Election> GetAll(bool includeDeactivated = false)
        {
            List<Election> election = new List<Election>();

            using (var ctx = GetVtsContext("ElectionRepository"))
            {
                if (includeDeactivated)
                    election =
                    CtxSetup(ctx.Elections)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    election =
                    CtxSetup(ctx.Elections)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return election;
        }

        public ValidationResultInfo Validate(Election itemToValidate)
        {
            return base.Validate(itemToValidate, GetAll(true).ToList());
        }
    }
}