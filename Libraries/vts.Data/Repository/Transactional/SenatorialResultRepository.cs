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
    public class SenatorialResultRepository : ISenatorialResultRepository
    {
        private readonly ContextConnection _contextConnection;

        public SenatorialResultRepository(ContextConnection contextConnection)
        {
            _contextConnection = contextConnection;
        }

        public void Save(SenatorialResult result)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ctx.UpdateGraph(result, map => map.OwnedCollection(n => n.LineItems));
                ctx.SaveChanges();
            }
        }

        public SenatorialResult GetById(Guid id)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                SenatorialResult results = CtxSetup(ctx.SenatorialResults)
                    .FirstOrDefault(n => n.Id == id);

                return results;
            }
        }

        public SenatorialResult GetByPollingCentre(PollingCentreRef pollingCentre)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                SenatorialResult results = CtxSetup(ctx.SenatorialResults)
                    .FirstOrDefault(n => n.PollingCentre.Id == pollingCentre.Id);

                return results;
            }
        }

        public List<SenatorialResult> GetAll()
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                List<SenatorialResult> doc = CtxSetup(ctx.SenatorialResults).ToList();
                return doc.ToList();
            }
        }

        IQueryable<SenatorialResult> CtxSetup(IQueryable<SenatorialResult> ctx)
        {
            return ctx.AsNoTracking()
                .Include(n => n.LineItems);
        }
    }
}