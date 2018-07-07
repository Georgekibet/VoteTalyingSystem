using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Commands
{
    public class CommandInfo
    {
        public string ResultReference { get; set; }
        public UserRef CommandGeneratedByUser { get; set; }
        public PollingCentreRef OriginatingPollingCentre { get; set; }
    }
}