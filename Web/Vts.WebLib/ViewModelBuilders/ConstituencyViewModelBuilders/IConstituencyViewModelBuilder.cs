using System;
using System.Collections.Generic;
using vts.Shared.Services;
using Vts.WebLib.ViewModels;

namespace Vts.WebLib.ViewModelBuilders.ConstituencyViewModelBuilders
{
    public interface IConstituencyViewModelBuilder
    {
        List<ConstituencyViewModel> GetAll(bool inactive = false);
        ConstituencyViewModel Get(Guid id);

        void Save(ConstituencyViewModel cvm);
        void SetInactive(Guid id);
        void SetActive(Guid id);
        void SetAsDeleted(Guid id);
        Dictionary<Guid, string> Counties();
        QueryResult<ConstituencyViewModel> Query(QueryStandard query);
    }
}
