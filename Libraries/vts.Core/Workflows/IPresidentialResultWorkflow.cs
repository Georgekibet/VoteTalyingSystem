using System;
using System.Collections.Generic;
using vts.Core.Commands;
using vts.Core.TransactionalEntities;

namespace vts.Core.Workflows
{
    public interface IPresidentialResultWorkflow
    {
        PresidentialResult Create(ResultInfo originatingInfo, string documentReference);

        PresidentialResult AddPresidentialResultLineItems(PresidentialResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);

        PresidentialResult Confirm(PresidentialResult result, ResultInfo originatingInfo);

        PresidentialResult Modify(PresidentialResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);
    }

    public class PresidentialResultWorkflow : IPresidentialResultWorkflow
    {
        public PresidentialResult Create(ResultInfo originatingInfo, string documentReference)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
                ResultReference = documentReference
            };
            CreatePresidentialResultCommand createPresidentialResultCommand = new CreatePresidentialResultCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = new ResultRef(Guid.NewGuid(), ResultType.Presidential),
                ResultReference = commandInfo.ResultReference,
                ResultDate = DateTime.Now,
                CommandExecutionOrder = 1
            };
            var result = new PresidentialResult();
            result.Apply(createPresidentialResultCommand);
            return result;
        }

        public PresidentialResult AddPresidentialResultLineItems(PresidentialResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            var command = new AddPresidentialLineItemsCommand
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

        public PresidentialResult Confirm(PresidentialResult result, ResultInfo originatingInfo)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            ConfirmPresidentialResultsCommand confirmPresidentialResultsCommand = new ConfirmPresidentialResultsCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = result.GetResultRef(),
                CommandExecutionOrder = result.LastResultCommandExecutedOrder + 1,
            };
            result.Apply(confirmPresidentialResultsCommand);
            return result;
        }

        public PresidentialResult Modify(PresidentialResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };

            var command = new ModifyPresidentialResultsCommand
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