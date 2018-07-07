using System.Linq;
using vts.Core.Contracts;

namespace vts.Shared.Services
{
    public class UnifiedDefaultServices : IDefaultServicesList
    {
        public UnifiedDefaultServices()
        {
            DefaultResultServices defaultResultServices = new DefaultResultServices();
            DefaultWorkflowServices defaultWorkflowServices = new DefaultWorkflowServices();

            Services = new DefaultServices();
            AddToServices(defaultResultServices);
            AddToServices(defaultWorkflowServices);
        }

        private void AddToServices(IDefaultServicesList list)
        {
            foreach (DefaultService s in list.Services.ServiceList)
            {
                string contract = s.Contract.FullName;
                int count = Services.ServiceList.Count(x => x.Contract.FullName == contract);
                if (count != 0)
                {
                    string message = string.Format("Attempt to register duplicate service - {0}", contract);
                    throw new DefaultServiceDuplicateException(message);
                }
                Services.ServiceList.Add(s);
            }
        }

        public DefaultServices Services { get; set; }
    }
}