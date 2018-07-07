using System;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Core.Commands
{
    public abstract class CreateCommand : Command
    {
        public DateTime ResultDate { get; set; }
        public string ResultReference { get; set; }
    }

    public abstract class Command
    {
        public Guid CommandId { get; set; }
        public ResultRef ApplyToResult { get; set; }
        public UserRef CommandGeneratedByUser { get; set; }
        public PollingCentreRef OriginatingPollingCentre { get; set; }
        public int CommandExecutionOrder { get; set; }
        public abstract CommandType CommandType { get; }
    }
}