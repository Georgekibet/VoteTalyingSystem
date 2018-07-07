using System.Collections.Generic;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class CreateGubernatorialResultCommand : CreateCommand
    {
        public override CommandType CommandType => CommandType.CreateGubernatorialResult;
    }

    public class AddGubernatorialLineItemsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.AddGubernatorialLineItems;
    }

    public class ConfirmGubernatorialResultsCommand : Command
    {
        public override CommandType CommandType => CommandType.ConfirmGubernatorialResults;
    }

    public class ModifyGubernatorialResultsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.ModifyGubernatorialResults;
    }
}