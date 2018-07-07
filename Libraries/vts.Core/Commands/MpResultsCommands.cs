using System.Collections.Generic;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class CreateMpResultCommand : CreateCommand
    {
        public override CommandType CommandType => CommandType.CreateMpResult;
    }

    public class AddMpLineItemsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.AddMpLineItems;
    }

    public class ConfirmMpResultsCommand : Command
    {
        public override CommandType CommandType => CommandType.ConfirmMpResults;
    }

    public class ModifyMpResultsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.ModifyMpResults;
    }
}