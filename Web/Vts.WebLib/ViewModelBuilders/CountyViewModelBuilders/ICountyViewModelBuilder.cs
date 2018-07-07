using System;
using System.Collections.Generic;
using vts.Shared.Services;
using Vts.WebLib.ViewModels.CountyViewModels;

namespace Vts.WebLib.ViewModelBuilders.CountyViewModelBuilders
{
    public interface ICountyViewModelBuilder
    {
        List<CountyViewModel> GetAll(bool inactive = false);
        CountyViewModel Get(Guid id);

        void Save(CountyViewModel countyViewModel);
        void SetInactive(Guid id);
        void SetActive(Guid id);
        void SetAsDeleted(Guid id);
        Dictionary<Guid, string> Regions();
        QueryResult<CountyViewModel> Query(QueryStandard query);
    }
}
