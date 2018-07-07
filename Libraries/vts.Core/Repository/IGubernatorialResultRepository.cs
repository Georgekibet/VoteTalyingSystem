using System;
using System.Collections.Generic;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Repository
{
    public interface IGubernatorialResultRepository
    {
        void Save(GubernatorialResult result);

        GubernatorialResult GetById(Guid id);

        GubernatorialResult GetByPollingCentre(PollingCentreRef pollingCentre);

        List<GubernatorialResult> GetAll();
    }
}