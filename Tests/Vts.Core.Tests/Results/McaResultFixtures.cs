using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using vts.Core.Commands;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace Vts.Core.Tests.Results
{
    [TestFixture, Category("vts.Core.Test.Results")]
    public class McaResultFixtures
    {
        [Test]
        public void ResultCreation_WhenCommandApplied_ValidNewResult()
        {
            var result = new McaResult();
            CreateMcaResultCommand cmd = DefaultCreateMcaResultCommand();
            //act
            result.Apply(cmd);
            //assert
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Id, Is.EqualTo(cmd.ApplyToResult.Id));
            Assert.That(result.Status, Is.EqualTo(ResultStatus.New));
            Assert.That(result.ResultReference, Is.EqualTo(cmd.ResultReference));
            Assert.That(result.PollingCentre, Is.EqualTo(cmd.OriginatingPollingCentre));
            Assert.That(result.ResultSender, Is.EqualTo(cmd.CommandGeneratedByUser));
            Assert.That(result.LineItems.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ResultAddLineItem_WhenCommandApplied_ValidLineItemAdded()
        {
            var result = new McaResult();
            CreateMcaResultCommand cmdCreate = DefaultCreateMcaResultCommand();
            result.Apply(cmdCreate);
            AddMcaLineItemsCommand cmd = DefaultAddMcaLineItemsCommand(2, cmdCreate.ApplyToResult, result.PollingCentre, result.ResultSender);
            //act
            result.Apply(cmd);
            //assert
            Assert.That(result.LineItems.Count(), Is.EqualTo(1));
            Assert.That(result.Id, Is.EqualTo(cmd.ApplyToResult.Id));
            Assert.That(result.Status, Is.EqualTo(ResultStatus.New));
            McaResultLineItem lineItem = result.LineItems[0];
            Assert.That(lineItem.Candidate, Is.EqualTo(cmd.ResultDetail[0].Candidate));
            Assert.That(lineItem.ResultCount, Is.EqualTo(cmd.ResultDetail[0].Result));
        }

        [Test]
        public void ResultConfirm_WhenCommandApplied_ResultStatusConfirmed()
        {
            var result = new McaResult();
            CreateMcaResultCommand cmdCreate = DefaultCreateMcaResultCommand();
            result.Apply(cmdCreate);
            AddMcaLineItemsCommand cmdLineItem = DefaultAddMcaLineItemsCommand(2, cmdCreate.ApplyToResult, result.PollingCentre, result.ResultSender);
            result.Apply(cmdLineItem);
            ConfirmMcaResultsCommand cmd = DefaultConfirmPresidentalResultsCommand(3, cmdLineItem.ApplyToResult, result.PollingCentre, result.ResultSender);
            //act
            result.Apply(cmd);
            //assert
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Confirmed));
        }

        [Test]
        public void ResultModify_WhenCommandApplied_ValidModifiedLineItemAdded()
        {
            var result = new McaResult();
            CreateMcaResultCommand cmdCreate = DefaultCreateMcaResultCommand();
            result.Apply(cmdCreate);
            AddMcaLineItemsCommand cmdLineItem = DefaultAddMcaLineItemsCommand(2, cmdCreate.ApplyToResult, result.PollingCentre, result.ResultSender);
            result.Apply(cmdLineItem);
            ConfirmMcaResultsCommand cmdConfirm = DefaultConfirmPresidentalResultsCommand(3, cmdLineItem.ApplyToResult, result.PollingCentre, result.ResultSender);
            result.Apply(cmdConfirm);
            ModifyMcaResultsCommand cmd = DefaultModifyMcaResultsCommand(4, cmdConfirm.ApplyToResult, result.PollingCentre, result.ResultSender);
            //act
            result.Apply(cmd);
            //assert
            Assert.That(result.LineItems.Count(), Is.EqualTo(2));
            Assert.That(result.Id, Is.EqualTo(cmd.ApplyToResult.Id));
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Modified));
            McaResultLineItem lineItem = result.LineItems[1];
            Assert.That(lineItem.Candidate, Is.EqualTo(cmd.ResultDetail[0].Candidate));
            Assert.That(lineItem.ResultCount, Is.EqualTo(cmd.ResultDetail[0].Result));
        }

        private CreateMcaResultCommand DefaultCreateMcaResultCommand()
        {
            return new CreateMcaResultCommand
            {
                CommandId = Guid.NewGuid(),
                ApplyToResult = new ResultRef(Guid.NewGuid(), ResultType.MemberOfCountyAssembly),
                CommandExecutionOrder = 1,
                OriginatingPollingCentre = new PollingCentreRef(Guid.NewGuid(), "Jamuhuri Primary"),
                CommandGeneratedByUser = new UserRef(Guid.NewGuid(), "Brian Mwasi", UserType.Clerk),
                ResultReference = "Test_Result",
                ResultDate = DateTime.Now
            };
        }

        private AddMcaLineItemsCommand DefaultAddMcaLineItemsCommand(int executionOrder, ResultRef result, PollingCentreRef pollingCentre, UserRef user)
        {
            var fixture = new Fixture();
            AddMcaLineItemsCommand cmd = fixture
                .Build<AddMcaLineItemsCommand>()
                .With(n => n.ApplyToResult, result)
                .Create();
            cmd.CommandId = Guid.NewGuid();
            cmd.ApplyToResult = result;
            CandidateRef candidate = new CandidateRef(Guid.NewGuid(), "John Doe", CandidateType.PartyBacked);
            var res = new ResultDetail { Candidate = candidate, Result = 1000 };
            var resList = new List<ResultDetail>();
            resList.Add(res);
            cmd.ResultDetail = resList;
            cmd.OriginatingPollingCentre = pollingCentre;
            cmd.CommandGeneratedByUser = user;
            cmd.CommandExecutionOrder = executionOrder;
            return cmd;
        }

        private ConfirmMcaResultsCommand DefaultConfirmPresidentalResultsCommand(int executionOrder, ResultRef result, PollingCentreRef pollingCentre, UserRef user)
        {
            return new ConfirmMcaResultsCommand
            {
                CommandId = Guid.NewGuid(),
                ApplyToResult = result,
                CommandGeneratedByUser = user,
                CommandExecutionOrder = executionOrder,
                OriginatingPollingCentre = pollingCentre
            };
        }

        private ModifyMcaResultsCommand DefaultModifyMcaResultsCommand(int executionOrder, ResultRef result, PollingCentreRef pollingCentre, UserRef user)
        {
            var fixture = new Fixture();
            ModifyMcaResultsCommand cmd = fixture
                .Build<ModifyMcaResultsCommand>()
                .With(n => n.ApplyToResult, result)
                .With(n => n.CommandGeneratedByUser, user)
                .With(n => n.OriginatingPollingCentre, pollingCentre)
                .Create();
            cmd.CommandId = Guid.NewGuid();
            cmd.ApplyToResult = result;
            CandidateRef candidate = new CandidateRef(Guid.NewGuid(), "John Doe", CandidateType.PartyBacked);
            var res = new ResultDetail { Candidate = candidate, Result = 1000 };
            var resList = new List<ResultDetail>();
            resList.Add(res);
            cmd.ResultDetail = resList;
            cmd.OriginatingPollingCentre = pollingCentre;
            cmd.CommandGeneratedByUser = user;
            cmd.CommandExecutionOrder = executionOrder;
            return cmd;
        }
    }
}