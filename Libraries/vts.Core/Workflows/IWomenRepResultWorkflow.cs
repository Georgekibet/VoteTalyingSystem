using System;
using System.Collections.Generic;
using vts.Core.Commands;
using vts.Core.TransactionalEntities;

namespace vts.Core.Workflows
{
    public interface IWomenRepResultWorkflow
    {
        WomenRepResult Create(ResultInfo originatingInfo, string documentReference);

        WomenRepResult AddWomenRepResultLineItems(WomenRepResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);

        WomenRepResult Confirm(WomenRepResult result, ResultInfo originatingInfo);

        WomenRepResult Modify(WomenRepResult result, ResultInfo originatingInfo, List<ResultDetail> resultDetails);
    }

    public class WomenRepResultWorkflow : IWomenRepResultWorkflow
    {
        public WomenRepResult Create(ResultInfo originatingInfo, string documentReference)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
                ResultReference = documentReference
            };
            CreateWomenRepResultCommand createWomenRepResultCommand = new CreateWomenRepResultCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = new ResultRef(Guid.NewGuid(), ResultType.WomenRepresentative),
                ResultReference = commandInfo.ResultReference,
                ResultDate = DateTime.Now,
                CommandExecutionOrder = 1
            };
            var result = new WomenRepResult();
            result.Apply(createWomenRepResultCommand);
            return result;
        }

        public WomenRepResult AddWomenRepResultLineItems(WomenRepResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            var command = new AddWomenRepLineItemsCommand
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

        public WomenRepResult Confirm(WomenRepResult result, ResultInfo originatingInfo)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };
            ConfirmWomenRepResultsCommand confirmWomenRepResultsCommand = new ConfirmWomenRepResultsCommand
            {
                CommandId = Guid.NewGuid(),
                CommandGeneratedByUser = commandInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = commandInfo.OriginatingPollingCentre,
                ApplyToResult = result.GetResultRef(),
                CommandExecutionOrder = result.LastResultCommandExecutedOrder + 1,
            };
            result.Apply(confirmWomenRepResultsCommand);
            return result;
        }

        public WomenRepResult Modify(WomenRepResult result, ResultInfo originatingInfo,
            List<ResultDetail> resultDetails)
        {
            CommandInfo commandInfo = new CommandInfo
            {
                CommandGeneratedByUser = originatingInfo.CommandGeneratedByUser,
                OriginatingPollingCentre = originatingInfo.OriginatingPollingCentre,
            };

            var command = new ModifyWomenRepResultsCommand
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