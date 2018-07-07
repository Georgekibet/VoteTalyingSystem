using vts.Core.Contracts;
using Vts.WebLib.ViewModelBuilders.ConstituencyViewModelBuilders;
using Vts.WebLib.ViewModelBuilders.ConstituencyViewModelBuilders.Impl;
using Vts.WebLib.ViewModelBuilders.CountyViewModelBuilders;
using Vts.WebLib.ViewModelBuilders.CountyViewModelBuilders.Impl;
using Vts.WebLib.ViewModelBuilders.PoliticalPartyViewModelBuilder;
using Vts.WebLib.ViewModelBuilders.RegionViewModelBuilders;

namespace Vts.WebLib.DefaultContracts
{
    public class ViewModelBuilderDefaultContracts : IDefaultServicesList
    {
        public ViewModelBuilderDefaultContracts()
        {
            Services = new DefaultServices()
                .Add<ICountyViewModelBuilder, CountyViewModelBuilder>()
                .Add<IRegionViewModelBuilder, RegionViewModelBuilder>()
                .Add<IConstituencyViewModelBuilder, ConstituencyViewModelBuilder>()
                .Add<IPoliticalPartyViewModelBuilder, PoliticalPartyViewModelBuilder>();
        }

        public DefaultServices Services { get; set; }
    }
}