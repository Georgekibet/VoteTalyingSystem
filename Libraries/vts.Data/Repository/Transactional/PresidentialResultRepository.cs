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
    public class PresidentialResultRepository : IPresidentialResultRepository
    {
        private readonly ContextConnection _contextConnection;

        public PresidentialResultRepository(ContextConnection contextConnection)
        {
            _contextConnection = contextConnection;
        }

        public void Save(PresidentialResult result)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                ctx.UpdateGraph(result, map => map.OwnedCollection(n => n.LineItems));
                ctx.SaveChanges();
            }
        }

        public PresidentialResult GetById(Guid id)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                PresidentialResult results = CtxSetup(ctx.PresidentialResults)
                    .FirstOrDefault(n => n.Id == id);
               
                return results;
            }
        }

        public PresidentialResult GetByPollingCentre(PollingCentreRef pollingCentre)
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                PresidentialResult results = CtxSetup(ctx.PresidentialResults)
                    .FirstOrDefault(n => n.PollingCentre.Id == pollingCentre.Id);

                return results;
            }
        }

        public List<PresidentialResult> GetAll()
        {
            using (var ctx = new VtsContext(_contextConnection.VtsConnectionString))
            {
                List<PresidentialResult> doc = CtxSetup(ctx.PresidentialResults).ToList();
                return doc.ToList();
            }
        }

        IQueryable<PresidentialResult> CtxSetup(IQueryable<PresidentialResult> ctx)
        {
            return ctx.AsNoTracking()
                .Include(n => n.LineItems);
        }
    }
}