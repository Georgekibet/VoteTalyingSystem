using System;
using System.Collections.Generic;
using vts.Core.Commands;
using vts.Core.TransactionalEntities;

namespace vts.Core.Workflows
{
    public interface IReferendumResultWorkflow
    {
        ReferendumResult Create(ResultInfo originatingInfo, string documentReference);

        ReferendumResult AddReferendumResultLineItems(ReferendumResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);

        ReferendumResult Confirm(ReferendumResult result, ResultInfo originatingInfo);

        ReferendumResult Modify(ReferendumResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);
    }

    public class ReferendumResultWorkflow : IReferendumResultWorkflow
    {
        public ReferendumResult Create(ResultInfo originatingInfo, string documentReference)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
                ResultReference = documentReference
            };
            CreateReferendumResultCommand createReferendumResultCommand = new CreateReferendumResultCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = new ResultRef(Guid.NewGuid(), ResultType.Referendum),
                ResultReference = commandInfo.ResultReference,
                ResultDate = DateTime.Now,
                CommandExecutionOrder = 1
            };
            var result = new ReferendumResult();
            result.Apply(createReferendumResultCommand);
            return result;
        }

        public ReferendumResult AddReferendumResultLineItems(ReferendumResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            var command = new AddReferendumLineItemsCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = result.GetResultRef(),
                CommandExecutionOrder = result.LastResultCommandExecutedOrder + 1,
                ResultDetail = resultDetails,
            };

            result.Apply(command);

            return result;
        }

        public ReferendumResult Confirm(ReferendumResult result, ResultInfo originatingInfo)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            ConfirmReferendumResultsCommand confirmReferendumResultsCommand = new ConfirmReferendumResultsCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = result.GetResultRef(),
                CommandExecutionOrder = result.LastResultCommandExecutedOrder + 1,
            };
            result.Apply(confirmReferendumResultsCommand);
            return result;
        }

        public ReferendumResult Modify(ReferendumResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };

            var command = new ModifyReferendumResultsCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = result.GetResultRef(),
                CommandExecutionOrder = result.LastResultCommandExecutedOrder + 1,
                ResultDetail = resultDetails,
            };
            result.Apply(command);
            return result;
        }
    }
}