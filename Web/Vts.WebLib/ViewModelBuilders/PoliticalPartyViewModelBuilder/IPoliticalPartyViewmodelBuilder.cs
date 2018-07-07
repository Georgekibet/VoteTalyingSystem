using System;
using System.Collections.Generic;
using Vts.WebLib.ViewModels;

namespace Vts.WebLib.ViewModelBuilders.PoliticalPartyViewModelBuilder
{
    public interface IPoliticalPartyViewModelBuilder
    {
        List<PoliticalPartyViewModel> GetAll(bool inactive = false);
        PoliticalPartyViewModel Get(Guid id);
        void Save(PoliticalPartyViewModel countyViewModel);
        void SetInactive(PoliticalPartyViewModel countyViewModel);
        void SetActive(PoliticalPartyViewModel countyViewModel);
        void SetAsDeleted(PoliticalPartyViewModel countyViewModel);
    }
}
