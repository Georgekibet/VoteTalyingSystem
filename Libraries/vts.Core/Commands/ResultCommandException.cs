using System;
using vts.Core.TransactionalEntities;

namespace vts.Core.Commands
{
    public class ResultCommandException : Exception
    {
        public ResultCommandException(Command command, ResultBase result, string message) : base(message)
        {
            CommandType = command.CommandType;
            CommandId = command.CommandId;
            ResultType = result.ResultType;
            ResultId = result.Id;
        }

        public Guid ResultId { get; set; }
        public Guid CommandId { get; set; }
        public CommandType CommandType { get; set; }
        public ResultType ResultType { get; set; }
    }
}
