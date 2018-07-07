using System;
using System.Collections.Generic;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Repository
{
    public interface IWomenRepResultRepository
    {
        void Save(WomenRepResult result);

        WomenRepResult GetById(Guid id);

        WomenRepResult GetByPollingCentre(PollingCentreRef pollingCentre);

        List<WomenRepResult> GetAll();
    }
}