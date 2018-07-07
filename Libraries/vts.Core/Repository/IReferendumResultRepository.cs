using System;
using System.Collections.Generic;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Repository
{
    public interface IReferendumResultRepository
    {
        void Save(ReferendumResult result);

        ReferendumResult GetById(Guid id);

        ReferendumResult GetByPollingCentre(PollingCentreRef pollingCentre);

        List<ReferendumResult> GetAll();
    }
}