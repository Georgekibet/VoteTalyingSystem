using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Core.Commands;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Entities.Master;

namespace vts.Core.TransactionalEntities
{
    public abstract class ResultBase
    {
        protected ResultBase(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public string ResultReference { get; set; }

        public UserRef ResultSender { get; set; }

        public PollingCentreRef PollingCentre { get; set; }

        public DateTime ResultSendDate { get; set; }

        public ResultStatus Status { get; set; }

        public abstract ResultType ResultType { get; }

        public List<Command> Commands = new List<Command>();

        public int LastResultCommandExecutedOrder { get; set; }

        public abstract void Apply(Command command);
        
        public ResultRef GetResultRef()
        {
            return new ResultRef(Id, ResultType);
        }

        protected void AddCommandToExecute(Command command)
        {
            int expectedOrder = LastResultCommandExecutedOrder + 1;
            if (expectedOrder != command.CommandExecutionOrder)
            {
                throw new ResultCommandException(command, this, "Invalid result command execution order");
            }
            this.LastResultCommandExecutedOrder++;
            Commands.Add(command);
        }

        protected void ValidateCreateCommand(CreateCommand command)
        {
            ValidateCommand(command);

            if (string.IsNullOrWhiteSpace(command.ResultReference))
            {
                throw new ResultCommandException(command, this, "Result Reference is required");
            }
            if (command.ResultDate == DateTime.MinValue)
            {
                throw new ResultCommandException(command, this, "Result Date should be set");
            }

            if (!command.ApplyToResult.IsValid().Item1)
            {
                throw new ResultCommandException(command, this, command.ApplyToResult.IsValid().Item2);
            }
        }

        protected void ValidateCommand(Command command)
        {
            if (command.CommandId == Guid.Empty)
            {
                throw new ResultCommandException(command, this,
                    "Invalid command id");
            }
            if (!command.ApplyToResult.IsValid().Item1)
            {
                throw new ResultCommandException(command, this,
                    command.ApplyToResult.IsValid().Item2);
            }
            if (!(command is CreateCommand))
            {
                if (command.ApplyToResult.Id != Id)
                    throw new ResultCommandException(command, this, "Invalid result id - should equal existing id");
            }

            if (!command.CommandGeneratedByUser.IsValid().Item1)
            {
                throw new ResultCommandException(command, this, command.CommandGeneratedByUser.IsValid().Item2);
            }

            if (this.ResultType != command.ApplyToResult.ResultType)
            {
                throw new ResultCommandException(command, this, String.Format("Invalid result type {0} in command ApplyToResult {1}", this.ResultType, command.ApplyToResult.ResultType));
            }
        }
    }

    [ComplexType]
    public class ResultRef
    {
        public ResultRef()
        {
            Id = Guid.Empty;
        }

        public ResultRef(Guid id, ResultType resultType)
        {
            Id = id;
            ResultType = resultType;
        }

        public Guid Id { get; set; }
        public ResultType ResultType { get; set; }

        public Tuple<bool, string> IsValid()
        {
            string message = "";
            if (Id == Guid.Empty)
                message += "Invalid result ref id,";
            if ((int)ResultType == 0)
                message += "Invalid result type";
            if (string.IsNullOrWhiteSpace(message)) return Tuple.Create(true, "");
            return Tuple.Create(false, message);
        }

        public static ResultRef CreateEmpty()
        {
            return new ResultRef(Guid.Empty, ResultType.None);
        }

        #region equality

        protected bool Equals(ResultRef other)
        {
            return Id.Equals(other.Id) && ResultType == other.ResultType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ResultRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (int)ResultType;
            }
        }

        #endregion equality
    }


    public class ResultDetail
    {
        public CandidateRef Candidate { get; set; }
        public int Result { get; set; }
    }
    public enum ResultType
    {
        None = 0,
        Presidential = 1,
        Gubernatorial = 2,
        Senatorial = 3,
        WomenRepresentative = 4,
        MemberOfParliament = 5,
        MemberOfCountyAssembly = 6,
        Referendum = 7,
    }

    public enum ResultStatus
    {
        None = 0,
        New = 1,
        Confirmed = 2,
        Modified = 3 
    }
}