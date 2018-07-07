using System;
using System.Collections.Generic;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Repository
{
    public interface IPresidentialResultRepository
    {
        void Save(PresidentialResult result);

        PresidentialResult GetById(Guid id);

        PresidentialResult GetByPollingCentre(PollingCentreRef pollingCentre);

        List<PresidentialResult> GetAll();
    }
}