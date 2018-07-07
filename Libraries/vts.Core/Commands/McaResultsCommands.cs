using System.Collections.Generic;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class CreateMcaResultCommand : CreateCommand
    {
        public override CommandType CommandType => CommandType.CreateMcaResult;
    }

    public class AddMcaLineItemsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.AddMcaLineItems;
    }

    public class ConfirmMcaResultsCommand : Command
    {
        public override CommandType CommandType => CommandType.ConfirmMcaResults;
    }

    public class ModifyMcaResultsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.ModifyMcaResults;
    }
}