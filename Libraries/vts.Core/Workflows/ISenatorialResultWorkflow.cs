using System;
using System.Collections.Generic;
using vts.Core.Commands;
using vts.Core.TransactionalEntities;

namespace vts.Core.Workflows
{
    public interface ISenatorialResultWorkflow
    {
        SenatorialResult Create(ResultInfo originatingInfo, string documentReference);

        SenatorialResult AddSenatorialResultLineItems(SenatorialResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);

        SenatorialResult Confirm(SenatorialResult result, ResultInfo originatingInfo);

        SenatorialResult Modify(SenatorialResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);
    }

    public class SenatorialResultWorkflow : ISenatorialResultWorkflow
    {
        public SenatorialResult Create(ResultInfo originatingInfo, string documentReference)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
                ResultReference = documentReference
            };
            CreateSenatorialResultCommand createSenatorialResultCommand = new CreateSenatorialResultCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = new ResultRef(Guid.NewGuid(), ResultType.Senatorial),
                ResultReference = commandInfo.ResultReference,
                ResultDate = DateTime.Now,
                CommandExecutionOrder = 1
            };
            var result = new SenatorialResult();
            result.Apply(createSenatorialResultCommand);
            return result;
        }

        public SenatorialResult AddSenatorialResultLineItems(SenatorialResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            var command = new AddSenatorialLineItemsCommand
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

        public SenatorialResult Confirm(SenatorialResult result, ResultInfo originatingInfo)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            ConfirmSenatorialResultsCommand confirmSenatorialResultsCommand = new ConfirmSenatorialResultsCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = result.GetResultRef(),
                CommandExecutionOrder = result.LastResultCommandExecutedOrder + 1,
            };
            result.Apply(confirmSenatorialResultsCommand);
            return result;
        }

        public SenatorialResult Modify(SenatorialResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };

            var command = new ModifySenatorialResultsCommand
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