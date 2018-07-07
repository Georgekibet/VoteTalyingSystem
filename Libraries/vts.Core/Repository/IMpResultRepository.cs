using System;
using System.Collections.Generic;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Repository
{
    public interface IMpResultRepository
    {
        void Save(MpResult result);

        MpResult GetById(Guid id);

        MpResult GetByPollingCentre(PollingCentreRef pollingCentre);

        List<MpResult> GetAll();
    }
}