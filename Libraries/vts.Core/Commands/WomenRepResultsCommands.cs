using System.Collections.Generic;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class CreateWomenRepResultCommand : CreateCommand
    {
        public override CommandType CommandType => CommandType.CreateWomenRepResult;
    }

    public class AddWomenRepLineItemsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.AddWomenRepLineItems;
    }

    public class ConfirmWomenRepResultsCommand : Command
    {
        public override CommandType CommandType => CommandType.ConfirmWomenRepResults;
    }

    public class ModifyWomenRepResultsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.ModifyWomenRepResults;
    }
}