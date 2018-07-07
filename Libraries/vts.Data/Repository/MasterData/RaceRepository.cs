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
    public class RaceRepository : BaseRepository<Race, RaceRef>, IRaceRepository
    {
        private readonly IElectionRepository _electionRepository;

        public RaceRepository(ContextConnection contextConnection, IElectionRepository electionRepository)
             : base(contextConnection)
        {
            _electionRepository = electionRepository;
        }

        protected override Func<Race, List<Race>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate Race Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<Race>, List<Race>> SearchFunc
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

        public Guid Save(Race entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Race not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("RaceRepository"))
            {
                Race race = GetById(entity.Id);
                if (race == null)
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

        private void SetStatus(Race entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("RaceRepository"))
            {
                Race c = ctx.Races.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(Race entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(Race entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(Race entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override Race GetById(Guid id, bool includeDeactivated = false)
        {
            Race race = null;
            using (var ctx = GetVtsContext("RaceRepository"))
            {
                race = CtxSetup(ctx.Races).FirstOrDefault(n => n.Id == id);
            }
            return race;
        }

        public override IEnumerable<Race> GetAll(bool includeDeactivated = false)
        {
            List<Race> race = new List<Race>();

            using (var ctx = GetVtsContext("RaceRepository"))
            {
                if (includeDeactivated)
                    race =
                    CtxSetup(ctx.Races)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    race =
                    CtxSetup(ctx.Races)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return race;
        }

        public ValidationResultInfo Validate(Race itemToValidate)
        {
            var validation = base.Validate(itemToValidate, GetAll(true).ToList());
            var elction = _electionRepository.GetById(itemToValidate.Election.Id);
            if (elction == null)
            {
                validation.Results.Add(new ValidationResult("Invalid election reference."));
            }
            return validation;
        }
    }
}