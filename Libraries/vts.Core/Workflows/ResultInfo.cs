using vts.Core.Shared.Entities.Master;
using vts.Shared.Entities.Master;

namespace vts.Core.Workflows
{
    public class ResultInfo
    {
        public UserRef CommandGeneratedByUser { get; set; }
        public PollingCentreRef OriginatingPollingCentre { get; set; }
    }
}