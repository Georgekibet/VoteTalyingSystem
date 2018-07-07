using System;
using System.Collections.Generic;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Repository
{
    public interface ISenatorialResultRepository
    {
        void Save(SenatorialResult result);

        SenatorialResult GetById(Guid id);

        SenatorialResult GetByPollingCentre(PollingCentreRef pollingCentre);

        List<SenatorialResult> GetAll();
    }
}