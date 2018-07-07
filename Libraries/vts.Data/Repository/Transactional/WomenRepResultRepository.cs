using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using vts.Core.Repository;
using vts.Core.TransactionalEntities;
using vts.Data.Context;
using vts.Shared.Entities.Master;

namespace vts.Data.Repository.Transactional
{
    public class WomenRepResultRepository : IWomenRepResultRepository
    {
        private readonly ContextConnection _contextConnection;

        public WomenRepResultRepository(ContextConnection contextConnection)
        {
            _contextConnection = contextConnection;
        }

        public void Save(WomenRepResult result)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ctx.UpdateGraph(result, map => map.OwnedCollection(n => n.LineItems));
                ctx.SaveChanges();
            }
        }

        public WomenRepResult GetById(Guid id)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                WomenRepResult results = CtxSetup(ctx.WomenRepResults)
                    .FirstOrDefault(n => n.Id == id);

                return results;
            }
        }

        public WomenRepResult GetByPollingCentre(PollingCentreRef pollingCentre)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                WomenRepResult results = CtxSetup(ctx.WomenRepResults)
                    .FirstOrDefault(n => n.PollingCentre.Id == pollingCentre.Id);

                return results;
            }
        }

        public List<WomenRepResult> GetAll()
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                List<WomenRepResult> doc = CtxSetup(ctx.WomenRepResults).ToList();
                return doc.ToList();
            }
        }

        IQueryable<WomenRepResult> CtxSetup(IQueryable<WomenRepResult> ctx)
        {
            return ctx.AsNoTracking()
                .Include(n => n.LineItems);
        }
    }
}