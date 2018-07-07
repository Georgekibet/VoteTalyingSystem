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
    public class CandidateRepository : BaseRepository<Candidate, CandidateRef>, ICandidateRepository
    {
        private readonly IPoliticalPartyRepository _politicalPartyRepository;
        private readonly IRaceRepository _raceRepository;

        public CandidateRepository(ContextConnection contextConnection, IPoliticalPartyRepository politicalPartyRepository, IRaceRepository raceRepository)
            : base(contextConnection)
        {
            _politicalPartyRepository = politicalPartyRepository;
            _raceRepository = raceRepository;
        }

        protected override Func<Candidate, List<Candidate>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.IdCardNumber == itemToCheck.IdCardNumber);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate Id Card Number found"));

                    var dupePassport = itemsToCheck.Any(n => n.PassportNumber == itemToCheck.PassportNumber);
                    if (dupePassport) validationResults.Add(new ValidationResult("Duplicate Passport Number found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<Candidate>, List<Candidate>> SearchFunc
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
                                n.Fullname().ToLower().Contains(st))
                            .ToList();
                };
            }
        }

        public Guid Save(Candidate entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Candidate not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("CandidateRepository"))
            {
                Candidate candidate = GetById(entity.Id);
                if (candidate == null)
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

        private void SetStatus(Candidate entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("CandidateRepository"))
            {
                Candidate c = ctx.Candidates.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(Candidate entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(Candidate entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(Candidate entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override Candidate GetById(Guid id, bool includeDeactivated = false)
        {
            Candidate candidate = null;
            using (var ctx = GetVtsContext("CandidateRepository"))
            {
                candidate = CtxSetup(ctx.Candidates).FirstOrDefault(n => n.Id == id);
            }
            return candidate;
        }

        public override IEnumerable<Candidate> GetAll(bool includeDeactivated = false)
        {
            List<Candidate> candidate = new List<Candidate>();

            using (var ctx = GetVtsContext("UserRepository"))
            {
                if (includeDeactivated)
                    candidate =
                    CtxSetup(ctx.Candidates)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    candidate =
                    CtxSetup(ctx.Candidates)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return candidate;
        }

        public ValidationResultInfo Validate(Candidate itemToValidate)
        {
            var validation = base.Validate(itemToValidate, GetAll(true).ToList());
            var race = _raceRepository.GetById(itemToValidate.Race.Id);
            var politicalParty = _politicalPartyRepository.GetById(itemToValidate.PoliticalParty.Id);
            if (race == null)
            {
                validation.Results.Add(new ValidationResult("Invalid race reference."));
            }
            if (politicalParty == null)
            {
                validation.Results.Add(new ValidationResult("Invalid political Party reference."));
            }
            return validation;
        }
    }
}