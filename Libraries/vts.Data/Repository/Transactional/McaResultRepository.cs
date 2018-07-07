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
    public class McaResultRepository : IMcaResultRepository
    {
        private readonly ContextConnection _contextConnection;

        public McaResultRepository(ContextConnection contextConnection)
        {
            _contextConnection = contextConnection;
        }

        public void Save(McaResult result)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ctx.UpdateGraph(result, map => map.OwnedCollection(n => n.LineItems));
                ctx.SaveChanges();
            }
        }

        public McaResult GetById(Guid id)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                McaResult results = CtxSetup(ctx.McaResults)
                    .FirstOrDefault(n => n.Id == id);

                return results;
            }
        }

        public McaResult GetByPollingCentre(PollingCentreRef pollingCentre)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                McaResult results = CtxSetup(ctx.McaResults)
                    .FirstOrDefault(n => n.PollingCentre.Id == pollingCentre.Id);

                return results;
            }
        }

        public List<McaResult> GetAll()
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                List<McaResult> doc = CtxSetup(ctx.McaResults).ToList();
                return doc.ToList();
            }
        }

        IQueryable<McaResult> CtxSetup(IQueryable<McaResult> ctx)
        {
            return ctx.AsNoTracking()
                .Include(n => n.LineItems);
        }
    }
}