using System;
using System.Collections.Generic;
using System.Linq;
using vts.Core.Commands;
using vts.Shared.Entities.Master;

namespace vts.Core.TransactionalEntities
{
    public class PresidentialResultLineItem
    {
        public Guid Id { get; set; }
        public CandidateRef Candidate { get; set; }
        public int ResultCount { get; set; }
        public int ModifiedCount { get; set; }
        public DateTime ReceivedTime { get; set; }
    }

    public class PresidentialResult : ResultBase
    {
        public PresidentialResult() : base(Guid.Empty)
        {
            LineItems = new List<PresidentialResultLineItem>();
        }

        public List<PresidentialResultLineItem> LineItems { get; set; }
        public override ResultType ResultType => ResultType.Presidential;

        public override void Apply(Command command)
        {
            switch (command.CommandType)
            {
                case CommandType.CreatePresidentialResult:
                    Create(command);
                    AddCommandToExecute(command);
                    break;

                case CommandType.AddPresidentialLineItems:
                    AddLineItem(command);
                    AddCommandToExecute(command);
                    break;

                case CommandType.ConfirmPresidentialResults:
                    Confirm(command);
                    AddCommandToExecute(command);
                    break;

                case CommandType.ModifyPresidentialResults:
                    Modify(command);
                    AddCommandToExecute(command);
                    break;

                default:
                    throw new ResultCommandException(command, this, "Invalid command passed to Presidential Result");
            }
        }

        private void Create(Command command)
        {
            var cmd = command as CreatePresidentialResultCommand;
            ValidateCreateCommand(cmd);
            if (cmd != null)
            {
                Id = cmd.ApplyToResult.Id;
                Status = ResultStatus.New;
                ResultReference = cmd.ResultReference;
                ResultSendDate = cmd.ResultDate;
                ResultSender = cmd.CommandGeneratedByUser;
                PollingCentre = cmd.OriginatingPollingCentre;
            }
        }

        private void AddLineItem(Command command)
        {
            var cmd = command as AddPresidentialLineItemsCommand;
            ValidateCommand(cmd);
            if (cmd != null)
            {
                foreach (var item in cmd.ResultDetail)
                {
                    var presidentalLineItem = new PresidentialResultLineItem()
                    {
                        Id = Guid.NewGuid(),
                        Candidate = item.Candidate,
                        ResultCount = item.Result,
                        ModifiedCount = 0,
                        ReceivedTime = DateTime.Now
                    };
                    LineItems.Add(presidentalLineItem);
                }
            }
        }

        private void Confirm(Command command)
        {
            var cmd = command as ConfirmPresidentialResultsCommand;
            ValidateCommand(cmd);
            Status = ResultStatus.Confirmed;
        }

        private void Modify(Command command)
        {
            //todo Review modify lineitems method
            var cmd = command as ModifyPresidentialResultsCommand;
            ValidateCommand(cmd);
            if (cmd != null)
            {
                foreach (var item in cmd.ResultDetail)
                {
                    var presidentalLineItem = new PresidentialResultLineItem()
                    {
                        Id = Guid.NewGuid(),
                        Candidate = item.Candidate,
                        ResultCount = item.Result,
                        ModifiedCount = LineItems.Max(z => z.ModifiedCount) + 1,
                        ReceivedTime = DateTime.Now
                    };
                    LineItems.Add(presidentalLineItem);
                }
            }
            Status = ResultStatus.Modified;
        }
    }
}