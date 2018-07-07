using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using vts.Core.Shared.Entities.Master;
using vts.Data.Context;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;

namespace vts.Data.Repository.MasterData
{
    public class ConstituencyRepository : BaseRepository<Constituency, ConstituencyRef>, IConstituencyRepository
    {
        private readonly ICountyRepository _countyRepository;

        public ConstituencyRepository(ContextConnection contextConnection, ICountyRepository countyRepository)
            : base(contextConnection)
        {
            _countyRepository = countyRepository;
        }

        protected override Func<Constituency, List<Constituency>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate Costituency Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<Constituency>, List<Constituency>> SearchFunc
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

        public Guid Save(Constituency entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Constituency not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("ConstituencyRepository"))
            {
                Constituency constituency = GetById(entity.Id);
                if (constituency == null)
                {
                    entity.Status = EntityStatus.Active;
                    entity.DateCreated = dt;
                }
                entity.DateLastUpdated = dt;
                ctx.UpdateGraph(entity, map => map.OwnedCollection(n => n.ConstituencyMps));

                ctx.SaveChanges();
            }

            return entity.Id;
        }

        private void SetStatus(Constituency entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("ConstituencyRepository"))
            {
                Constituency c = ctx.Constituencies.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(Constituency entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(Constituency entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(Constituency entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override Constituency GetById(Guid id, bool includeDeactivated = false)
        {
            Constituency constituency = null;
            using (var ctx = GetVtsContext("ConstituencyRepository"))
            {
                constituency = CtxSetup(ctx.Constituencies).FirstOrDefault(n => n.Id == id);
            }
            return constituency;
        }

        public override IEnumerable<Constituency> GetAll(bool includeDeactivated = false)
        {
            List<Constituency> constituency = new List<Constituency>();

            using (var ctx = GetVtsContext("ConstituencyRepository"))
            {
                if (includeDeactivated)
                    constituency =
                    CtxSetup(ctx.Constituencies)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    constituency =
                    CtxSetup(ctx.Constituencies)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return constituency;
        }

        public QueryResult<Constituency> Query(QueryStandard query)
        {
            var result = new QueryResult<Constituency>();
            using (var ctx = GetVtsContext("ConstituencyRepository"))
            {
                var qryResult = ctx.Constituencies.AsQueryable().AsNoTracking();
                qryResult = query.IncludeDeactivated
                    ? qryResult.Where(n => n.Status != EntityStatus.Deleted)
                    : qryResult.Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive);
                if (!string.IsNullOrEmpty(query.Description))
                {
                    string search = query.Description.ToLower();
                    qryResult =
                        qryResult.Where(
                            n =>
                                n.Name.ToLower().Contains(search));
                }
                var r = qryResult.OrderBy(n => n.Name).ToList();
                result.Count = r.Count();
                if (query.Skip.HasValue && query.Take.HasValue)
                    r = r.Skip(query.Skip.Value).Take(query.Take.Value).ToList();
                result.Data = r;
            }
            return result;
        }

        public ValidationResultInfo Validate(Constituency itemToValidate)
        {
            var validation = base.Validate(itemToValidate, GetAll(true).ToList());
            var county = _countyRepository.GetById(itemToValidate.County.Id);
            if (county == null)
            {
                validation.Results.Add(new ValidationResult("Invalid constituency reference."));
            }
            return validation;
        }
    }
}