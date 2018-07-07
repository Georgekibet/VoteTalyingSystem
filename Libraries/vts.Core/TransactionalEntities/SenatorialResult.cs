using System;
using System.Collections.Generic;
using System.Linq;
using vts.Core.Commands;
using vts.Shared.Entities.Master;

namespace vts.Core.TransactionalEntities
{
    public class SenatorialResultLineItem
    {
        public Guid Id { get; set; }
        public CandidateRef Candidate { get; set; }
        public int ResultCount { get; set; }
        public int ModifiedCount { get; set; }
        public DateTime ReceivedTime { get; set; }
    }

    public class SenatorialResult : ResultBase
    {
        public SenatorialResult() : base(Guid.Empty)
        {
            LineItems = new List<SenatorialResultLineItem>();
        }

        public List<SenatorialResultLineItem> LineItems { get; set; }
        public override ResultType ResultType => ResultType.Senatorial;

        public override void Apply(Command command)
        {
            switch (command.CommandType)
            {
                case CommandType.CreateSenatorialResult:
                    Create(command);
                    AddCommandToExecute(command);
                    break;

                case CommandType.AddSenatorialLineItems:
                    AddLineItem(command);
                    AddCommandToExecute(command);
                    break;

                case CommandType.ConfirmSenatorialResults:
                    Confirm(command);
                    AddCommandToExecute(command);
                    break;

                case CommandType.ModifySenatorialResults:
                    Modify(command);
                    AddCommandToExecute(command);
                    break;

                default:
                    throw new ResultCommandException(command, this, "Invalid command passed to Senatorial Result");
            }
        }

        private void Create(Command command)
        {
            var cmd = command as CreateSenatorialResultCommand;
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
            var cmd = command as AddSenatorialLineItemsCommand;
            ValidateCommand(cmd);
            if (cmd != null)
            {
                foreach (var item in cmd.ResultDetail)
                {
                    var presidentalLineItem = new SenatorialResultLineItem()
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
            var cmd = command as ConfirmSenatorialResultsCommand;
            ValidateCommand(cmd);
            Status = ResultStatus.Confirmed;
        }

        private void Modify(Command command)
        {
            //todo Review modify lineitems method
            var cmd = command as ModifySenatorialResultsCommand;
            ValidateCommand(cmd);
            if (cmd != null)
            {
                foreach (var item in cmd.ResultDetail)
                {
                    var presidentalLineItem = new SenatorialResultLineItem()
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