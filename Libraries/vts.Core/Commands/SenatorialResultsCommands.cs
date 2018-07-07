using System.Collections.Generic;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class CreateSenatorialResultCommand : CreateCommand
    {
        public override CommandType CommandType => CommandType.CreateSenatorialResult;
    }

    public class AddSenatorialLineItemsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.AddSenatorialLineItems;
    }

    public class ConfirmSenatorialResultsCommand : Command
    {
        public override CommandType CommandType => CommandType.ConfirmSenatorialResults;
    }

    public class ModifySenatorialResultsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.ModifySenatorialResults;
    }
}