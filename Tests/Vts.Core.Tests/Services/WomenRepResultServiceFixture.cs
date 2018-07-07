using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using vts.Core.Repository;
using vts.Core.ResultServices;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace Vts.Core.Tests.Services
{
    [TestFixture, Category("vts.Core.Test.Services")]
    public class WomenRepResultServiceFixture
    {
        private static IContainer _ioc;

        [SetUp]
        public void Setup()
        {
            _ioc = IocHelper.DefaultContainerAutofac();
        }

        [Test]
        public void ResultProcessing_WhenReceived_ValidResultSaved()
        {
            //Arrange
            UserRef user = new UserRef(Guid.NewGuid(), "Brian Mwasi", UserType.Clerk);
            PollingCentreRef pollingCentre = new PollingCentreRef(Guid.NewGuid(), "Jamuhuri Primary");
            ResultDetail resultDetail = new ResultDetail { Candidate = new CandidateRef(Guid.NewGuid(), "Shebesh", CandidateType.PartyBacked), Result = 1000 };
            ResultDetail resultDetail1 = new ResultDetail { Candidate = new CandidateRef(Guid.NewGuid(), "Passaris", CandidateType.PartyBacked), Result = 2000 };
            List<ResultDetail> resultDetails = new List<ResultDetail>();
            resultDetails.Add(resultDetail);
            resultDetails.Add(resultDetail1);
            IWomenRepResultService womenRepResultService = _ioc.Resolve<IWomenRepResultService>();
            IWomenRepResultRepository womenRepResultRepository = _ioc.Resolve<IWomenRepResultRepository>();
            //Act
            womenRepResultService.Excecute(user, pollingCentre, resultDetails);
            //Assert
            var womenRepResult = womenRepResultRepository.GetAll().OrderByDescending(n => n.ResultSendDate).First();
            Assert.That(womenRepResult.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.IsNotNull(womenRepResult.ResultReference);
            Assert.That(womenRepResult.ResultSender, Is.EqualTo(user));
            Assert.That(womenRepResult.PollingCentre, Is.EqualTo(pollingCentre));
            Assert.That(womenRepResult.Status, Is.EqualTo(ResultStatus.Confirmed));
            Assert.That(womenRepResult.ResultSender, Is.EqualTo(user));
            Assert.That(womenRepResult.LineItems.OrderBy(n => n.Candidate.FullName).First().Candidate, Is.EqualTo(resultDetail1.Candidate));
            Assert.That(womenRepResult.LineItems.OrderBy(n => n.Candidate.FullName).Last().Candidate, Is.EqualTo(resultDetail.Candidate));
            Assert.That(womenRepResult.LineItems.OrderBy(n => n.Candidate.FullName).First().ResultCount, Is.EqualTo(resultDetail1.Result));
            Assert.That(womenRepResult.LineItems.OrderBy(n => n.Candidate.FullName).Last().ResultCount, Is.EqualTo(resultDetail.Result));
        }
    }
}