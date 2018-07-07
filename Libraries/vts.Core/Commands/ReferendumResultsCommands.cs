using System.Collections.Generic;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class CreateReferendumResultCommand : CreateCommand
    {
        public override CommandType CommandType => CommandType.CreateReferendumResult;
    }

    public class AddReferendumLineItemsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.AddReferendumLineItems;
    }

    public class ConfirmReferendumResultsCommand : Command
    {
        public override CommandType CommandType => CommandType.ConfirmReferendumResults;
    }

    public class ModifyReferendumResultsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.ModifyReferendumResults;
    }
}