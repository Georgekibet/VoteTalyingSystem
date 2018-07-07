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
    public class GubernatorialResultRepository : IGubernatorialResultRepository
    {
        private readonly ContextConnection _contextConnection;

        public GubernatorialResultRepository(ContextConnection contextConnection)
        {
            _contextConnection = contextConnection;
        }

        public void Save(GubernatorialResult result)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ctx.UpdateGraph(result, map => map.OwnedCollection(n => n.LineItems));
                ctx.SaveChanges();
            }
        }

        public GubernatorialResult GetById(Guid id)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                GubernatorialResult results = CtxSetup(ctx.GubernatorialResults)
                    .FirstOrDefault(n => n.Id == id);

                return results;
            }
        }

        public GubernatorialResult GetByPollingCentre(PollingCentreRef pollingCentre)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                GubernatorialResult results = CtxSetup(ctx.GubernatorialResults)
                    .FirstOrDefault(n => n.PollingCentre.Id == pollingCentre.Id);

                return results;
            }
        }

        public List<GubernatorialResult> GetAll()
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                List<GubernatorialResult> doc = CtxSetup(ctx.GubernatorialResults).ToList();
                return doc.ToList();
            }
        }

        IQueryable<GubernatorialResult> CtxSetup(IQueryable<GubernatorialResult> ctx)
        {
            return ctx.AsNoTracking()
                .Include(n => n.LineItems);
        }
    }
}