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
    public class RegionRepository : BaseRepository<Region, RegionRef>, IRegionRepository
    {
        public RegionRepository(ContextConnection contextConnection)
            : base(contextConnection)
        {
        }

        protected override Func<Region, List<Region>, ValidationResult[]> ValidationFunc
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                return (itemToCheck, allItems) =>
                {
                    var itemsToCheck = allItems.Where(n => n.Id != itemToCheck.Id);

                    var dupeId = itemsToCheck.Any(n => n.Name == itemToCheck.Name);
                    if (dupeId) validationResults.Add(new ValidationResult("Duplicate Region Name found"));

                    var validation = itemToCheck.Validate();
                    if (!validation.IsValid)
                        validationResults.AddRange(validation.Results);
                    return validationResults.ToArray();
                };
            }
        }

        protected override Func<string, List<Region>, List<Region>> SearchFunc
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

        public Guid Save(Region entity, bool? isSync = null)
        {
            var vri = new ValidationResultInfo();

            if (isSync == null || !isSync.Value)
            {
                vri = Validate(entity);
            }

            if (!vri.IsValid)
            {
                throw new DomainValidationException(vri, "Region not valid");
            }
            DateTime dt = DateTime.Now;
            using (var ctx = GetVtsContext("RegionRepository"))
            {
                Region region = GetById(entity.Id);
                if (region == null)
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

        public Region GetByName(string regionName, bool includeDeactivated = false)
        {
            Region region = null;
            using (var ctx = GetVtsContext("RegionRepository"))
            {
                region = CtxSetup(ctx.Regions).FirstOrDefault(n => n.Name == regionName);
            }
            return region;
        }

        private void SetStatus(Region entity, EntityStatus status)
        {
            using (var ctx = GetVtsContext("RegionRepository"))
            {
                Region c = ctx.Regions.Find(entity.Id);
                if (c != null)
                {
                    if (c.Status == status) return;
                    c.Status = status;
                    c.DateLastUpdated = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        public void SetInactive(Region entity)
        {
            SetStatus(entity, EntityStatus.Inactive);
        }

        public void SetActive(Region entity)
        {
            SetStatus(entity, EntityStatus.Active);
        }

        public void SetAsDeleted(Region entity)
        {
            SetStatus(entity, EntityStatus.Deleted);
        }

        public override Region GetById(Guid id, bool includeDeactivated = false)
        {
            Region region = null;
            using (var ctx = GetVtsContext("RegionRepository"))
            {
                region = CtxSetup(ctx.Regions).FirstOrDefault(n => n.Id == id);
            }
            return region;
        }

        public override IEnumerable<Region> GetAll(bool includeDeactivated = false)
        {
            List<Region> region = new List<Region>();

            using (var ctx = GetVtsContext("RegionRepository"))
            {
                if (includeDeactivated)
                    region =
                    CtxSetup(ctx.Regions)
                        .Where(n => n.Status != EntityStatus.Deleted)
                        .ToList();
                else
                    region =
                    CtxSetup(ctx.Regions)
                        .Where(n => n.Status != EntityStatus.Deleted && n.Status != EntityStatus.Inactive)
                        .ToList();
            }
            return region;
        }

        public QueryResult<Region> Query(QueryStandard query)
        {
            var result = new QueryResult<Region>();
            using (var ctx = GetVtsContext("RegionRepository"))
            {
                var qryResult = ctx.Regions.AsQueryable().AsNoTracking();
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

        public ValidationResultInfo Validate(Region itemToValidate)
        {
            return base.Validate(itemToValidate, GetAll(true).ToList());
        }
    }
}