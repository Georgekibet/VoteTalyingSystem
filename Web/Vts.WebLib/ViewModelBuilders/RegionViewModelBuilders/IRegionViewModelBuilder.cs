using System;
using System.Collections.Generic;
using vts.Shared.Services;
using Vts.WebLib.ViewModels;

namespace Vts.WebLib.ViewModelBuilders.RegionViewModelBuilders
{
    public interface IRegionViewModelBuilder
    {
        List<RegionViewModel> GetAll(bool inactive = false);
        RegionViewModel Get(Guid id);

        void Save(RegionViewModel regionViewModel);
        void SetInactive(Guid id);
        void SetActive(Guid id);
        void SetAsDeleted(Guid id);
        QueryResult<RegionViewModel> Query(QueryStandard query);
    }
}
