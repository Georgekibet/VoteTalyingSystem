using vts.Core.ResultServices;

namespace vts.Core.Contracts
{
    internal class DefaultResultServices : IDefaultServicesList
    {
        public DefaultResultServices()
        {
            Services = new DefaultServices()
                .Add<IPresidentialResultService, PresidentialResultService>()
                .Add<IGubernatorialResultService, GubernatorialResultService>()
                .Add<ISenatorialResultService, SenatorialResultService>()
                .Add<IMpResultService, MpResultService>()
                .Add<IMcaResultService, McaResultService>()
                .Add<IWomenRepResultService, WomenRepResultService>()
                .Add<IReferendumResultService, ReferendumResultService>()
                ;
        }

        public DefaultServices Services { get; set; }
    }
}