using System.Collections.Generic;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class CreatePresidentialResultCommand : CreateCommand
    {
        public override CommandType CommandType => CommandType.CreatePresidentialResult;
    }

    public class AddPresidentialLineItemsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.AddPresidentialLineItems;
    }

    public class ConfirmPresidentialResultsCommand : Command
    {
        public override CommandType CommandType => CommandType.ConfirmPresidentialResults;
    }

    public class ModifyPresidentialResultsCommand : Command
    {
        public List<ResultDetail> ResultDetail { get; set; }
        public override CommandType CommandType => CommandType.ModifyPresidentialResults;
    }
}