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
    public class ReferendumResultRepository : IReferendumResultRepository
    {
        private readonly ContextConnection _contextConnection;

        public ReferendumResultRepository(ContextConnection contextConnection)
        {
            _contextConnection = contextConnection;
        }

        public void Save(ReferendumResult result)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ctx.UpdateGraph(result, map => map.OwnedCollection(n => n.LineItems));
                ctx.SaveChanges();
            }
        }

        public ReferendumResult GetById(Guid id)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ReferendumResult results = CtxSetup(ctx.ReferendumResults)
                    .FirstOrDefault(n => n.Id == id);

                return results;
            }
        }

        public ReferendumResult GetByPollingCentre(PollingCentreRef pollingCentre)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ReferendumResult results = CtxSetup(ctx.ReferendumResults)
                    .FirstOrDefault(n => n.PollingCentre.Id == pollingCentre.Id);

                return results;
            }
        }

        public List<ReferendumResult> GetAll()
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                List<ReferendumResult> doc = CtxSetup(ctx.ReferendumResults).ToList();
                return doc.ToList();
            }
        }

        IQueryable<ReferendumResult> CtxSetup(IQueryable<ReferendumResult> ctx)
        {
            return ctx.AsNoTracking()
                .Include(n => n.LineItems);
        }
    }
}