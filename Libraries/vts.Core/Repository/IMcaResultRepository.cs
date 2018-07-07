using System;
using System.Collections.Generic;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Repository
{
    public interface IMcaResultRepository
    {
        void Save(McaResult result);

        McaResult GetById(Guid id);

        McaResult GetByPollingCentre(PollingCentreRef pollingCentre);

        List<McaResult> GetAll();
    }
}