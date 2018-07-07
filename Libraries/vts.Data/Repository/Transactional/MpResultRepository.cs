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
    public class MpResultRepository : IMpResultRepository
    {
        private readonly ContextConnection _contextConnection;

        public MpResultRepository(ContextConnection contextConnection)
        {
            _contextConnection = contextConnection;
        }

        public void Save(MpResult result)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ctx.UpdateGraph(result, map => map.OwnedCollection(n => n.LineItems));
                ctx.SaveChanges();
            }
        }

        public MpResult GetById(Guid id)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                MpResult results = CtxSetup(ctx.MpResults)
                    .FirstOrDefault(n => n.Id == id);

                return results;
            }
        }

        public MpResult GetByPollingCentre(PollingCentreRef pollingCentre)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                MpResult results = CtxSetup(ctx.MpResults)
                    .FirstOrDefault(n => n.PollingCentre.Id == pollingCentre.Id);

                return results;
            }
        }

        public List<MpResult> GetAll()
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                List<MpResult> doc = CtxSetup(ctx.MpResults).ToList();
                return doc.ToList();
            }
        }

        IQueryable<MpResult> CtxSetup(IQueryable<MpResult> ctx)
        {
            return ctx.AsNoTracking()
                .Include(n => n.LineItems);
        }
    }
}