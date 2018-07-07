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
    public class PoliticalPartyRepository : BaseRepository<PoliticalParty, PoliticalPartyRef>, IPoliticalPartyRepository
    {
        public PoliticalPartyRepository(ContextConnection contextConnection)
             : base(contextConnection)
        {
        }

        protected override Func<PoliticalParty, List<PoliticalParty>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate PoliticalParty Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<PoliticalParty>, List<PoliticalParty>> SearchFunc
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

        public Guid Save(PoliticalParty entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "PoliticalParty not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("PoliticalPartyRepository"))
            {
                PoliticalParty politicalParty = GetById(entity.Id);
                if (politicalParty == null)
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

        private void SetStatus(PoliticalParty entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("PoliticalPartyRepository"))
            {
                PoliticalParty c = ctx.PoliticalParties.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(PoliticalParty entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(PoliticalParty entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(PoliticalParty entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override PoliticalParty GetById(Guid id, bool includeDeactivated = false)
        {
            PoliticalParty politicalParty = null;
            using (var ctx = GetVtsContext("PoliticalPartyRepository"))
            {
                politicalParty = CtxSetup(ctx.PoliticalParties).FirstOrDefault(n => n.Id == id);
            }
            return politicalParty;
        }

        public override IEnumerable<PoliticalParty> GetAll(bool includeDeactivated = false)
        {
            List<PoliticalParty> politicalParty = new List<PoliticalParty>();

            using (var ctx = GetVtsContext("PoliticalPartyRepository"))
            {
                if (includeDeactivated)
                    politicalParty =
                    CtxSetup(ctx.PoliticalParties)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    politicalParty =
                    CtxSetup(ctx.PoliticalParties)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return politicalParty;
        }

        public ValidationResultInfo Validate(PoliticalParty itemToValidate)
        {
            return base.Validate(itemToValidate, GetAll(true).ToList());
        }
    }
}