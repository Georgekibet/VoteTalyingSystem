using vts.Core.Workflows;

namespace vts.Core.Contracts
{
    public class DefaultWorkflowServices : IDefaultServicesList
    {
        public DefaultWorkflowServices()
        {
            Services = new DefaultServices()
                .Add<IPresidentialResultWorkflow, PresidentialResultWorkflow>()
                .Add<IGubernatorialResultWorkflow, GubernatorialResultWorkflow>()
                .Add<ISenatorialResultWorkflow, SenatorialResultWorkflow>()
                .Add<IMpResultWorkflow, MpResultWorkflow>()
                .Add<IMcaResultWorkflow, McaResultWorkflow>()
                .Add<IWomenRepResultWorkflow, WomenRepResultWorkflow>()
                .Add<IReferendumResultWorkflow, ReferendumResultWorkflow>()
                ;
        }

        public DefaultServices Services { get; set; }
    }
}