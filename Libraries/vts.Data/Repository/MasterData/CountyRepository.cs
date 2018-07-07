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
    public class CountyRepository : BaseRepository<County, CountyRef>, ICountyRepository
    {
        private readonly IRegionRepository _regionRepository;

        public CountyRepository(ContextConnection contextConnection, IRegionRepository regionRepository) : base(contextConnection)
        {
            _regionRepository = regionRepository;
        }

        protected override Func<County, List<County>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate County Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<County>, List<County>> SearchFunc
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

        public Guid Save(County entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "County not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("CountyRepository"))
            {
                County county = GetById(entity.Id);
                if (county == null)
                {
                    entity.Status = EntityStatus.Active;
                    entity.DateCreated = dt;
                }
                entity.DateLastUpdated = dt;
                ctx.UpdateGraph(entity, map => map.OwnedCollection(n => n.CountyGovernors)
                .OwnedCollection(n => n.CountySenators)
                .OwnedCollection(n => n.CountyWomenReps));
                ctx.SaveChanges();
            }

            return entity.Id;
        }

        private void SetStatus(County entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("CountyRepository"))
            {
                County c = ctx.Counties.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(County entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(County entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(County entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override County GetById(Guid id, bool includeDeactivated = false)
        {
            County county = null;
            using (var ctx = GetVtsContext("CountyRepository"))
            {
                county = CtxSetup(ctx.Counties).FirstOrDefault(n => n.Id == id);
            }
            return county;
        }

        public override IEnumerable<County> GetAll(bool includeDeactivated = false)
        {
            List<County> county = new List<County>();

            using (var ctx = GetVtsContext("CountyRepository"))
            {
                if (includeDeactivated)
                    county =
                    CtxSetup(ctx.Counties)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    county =
                    CtxSetup(ctx.Counties)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return county;
        }

        public QueryResult<County> Query(QueryStandard query)
        {
            var result = new QueryResult<County>();
            using (var ctx = GetVtsContext("CountyRepository"))
            {
                var qryResult = ctx.Counties.AsQueryable().AsNoTracking();
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

        public ValidationResultInfo Validate(County itemToValidate)
        {
            var validation = base.Validate(itemToValidate, GetAll(true).ToList());
            var region = _regionRepository.GetById(itemToValidate.Region.Id);
            if (region == null)
            {
                validation.Results.Add(new ValidationResult("Invalid region reference."));
            }
            return validation;
        }
    }
}