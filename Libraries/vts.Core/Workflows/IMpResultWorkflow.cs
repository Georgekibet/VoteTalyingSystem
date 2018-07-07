using System;
using System.Collections.Generic;
using vts.Core.Commands;
using vts.Core.TransactionalEntities;

namespace vts.Core.Workflows
{
    public interface IMpResultWorkflow
    {
        MpResult Create(ResultInfo originatingInfo, string documentReference);

        MpResult AddMpResultLineItems(MpResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);

        MpResult Confirm(MpResult result, ResultInfo originatingInfo);

        MpResult Modify(MpResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);
    }

    public class MpResultWorkflow : IMpResultWorkflow
    {
        public MpResult Create(ResultInfo originatingInfo, string documentReference)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
                ResultReference = documentReference
            };
            CreateMpResultCommand createMpResultCommand = new CreateMpResultCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = new ResultRef(Guid.NewGuid(), ResultType.MemberOfParliament),
                ResultReference = commandInfo.ResultReference,
                ResultDate = DateTime.Now,
                CommandExecutionOrder = 1
            };
            var result = new MpResult();
            result.Apply(createMpResultCommand);
            return result;
        }

        public MpResult AddMpResultLineItems(MpResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            var command = new AddMpLineItemsCommand
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

        public MpResult Confirm(MpResult result, ResultInfo originatingInfo)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            ConfirmMpResultsCommand confirmMpResultsCommand = new ConfirmMpResultsCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = result.GetResultRef(),
                CommandExecutionOrder = result.LastResultCommandExecutedOrder + 1,
            };
            result.Apply(confirmMpResultsCommand);
            return result;
        }

        public MpResult Modify(MpResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };

            var command = new ModifyMpResultsCommand
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