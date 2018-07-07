using NUnit.Framework;
using System;
using System.Linq;
using NSubstitute;
using Ploeh.AutoFixture;
using vts.Core.Shared.Entities.Master;
using vts.Data.Repository;
using vts.Data.Repository.MasterData;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;

namespace Vts.Core.Tests.Repository
{
    [TestFixture, Category("VtsIntegrationTests"), Category("IT_RepositoryFixtures")]
    public class CandidateRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_Candidate()
        {
            IRaceRepository raceRepository = Substitute.For<IRaceRepository>();
            IPoliticalPartyRepository politicalPartyRepository = Substitute.For<IPoliticalPartyRepository>();
            var f = new Fixture();
            var race = f.Create<Race>();
            var politicalParty = f.Create<PoliticalParty>();
            var candidate = Create();
            raceRepository.GetById(Arg.Any<Guid>()).Returns(race);
            politicalPartyRepository.GetById(Arg.Any<Guid>()).Returns(politicalParty);
            var candidateRepository = new CandidateRepository(ContextConnection(), politicalPartyRepository,raceRepository );
            var id = candidateRepository.Save(candidate);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, candidate.Id);
        }

        [Test]
        public void SimpeDeHydrate_Candidate()
        {
            IRaceRepository raceRepository = Substitute.For<IRaceRepository>();
            IPoliticalPartyRepository politicalPartyRepository = Substitute.For<IPoliticalPartyRepository>();
            var f = new Fixture();
            var race = f.Create<Race>();
            var politicalParty = f.Create<PoliticalParty>();
            var candidate = Create();
            raceRepository.GetById(Arg.Any<Guid>()).Returns(race);
            politicalPartyRepository.GetById(Arg.Any<Guid>()).Returns(politicalParty);
            var candidateRepository = new CandidateRepository(ContextConnection(), politicalPartyRepository, raceRepository);
            var id = candidateRepository.Save(candidate);
            var owner = candidateRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, candidate.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_Candidate()
        {
            IRaceRepository raceRepository = Substitute.For<IRaceRepository>();
            IPoliticalPartyRepository politicalPartyRepository = Substitute.For<IPoliticalPartyRepository>();
            var f = new Fixture();
            var race = f.Create<Race>();
            var politicalParty = f.Create<PoliticalParty>();
            var candidate = Create();
            raceRepository.GetById(Arg.Any<Guid>()).Returns(race);
            politicalPartyRepository.GetById(Arg.Any<Guid>()).Returns(politicalParty);
            var candidateRepository = new CandidateRepository(ContextConnection(), politicalPartyRepository, raceRepository);
            candidateRepository.Save(candidate);
            var owner = candidateRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_Candidate()
        {
            IRaceRepository raceRepository = Substitute.For<IRaceRepository>();
            IPoliticalPartyRepository politicalPartyRepository = Substitute.For<IPoliticalPartyRepository>();
            var f = new Fixture();
            var race = f.Create<Race>();
            var politicalParty = f.Create<PoliticalParty>();
            var candidate = Create();
            raceRepository.GetById(Arg.Any<Guid>()).Returns(race);
            politicalPartyRepository.GetById(Arg.Any<Guid>()).Returns(politicalParty);
            var candidateRepository = new CandidateRepository(ContextConnection(), politicalPartyRepository, raceRepository);
            candidateRepository.Save(candidate);
            candidateRepository.SetInactive(candidate);
            var inactive = candidateRepository.GetById(candidate.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            candidateRepository.SetActive(candidate);
            var active = candidateRepository.GetById(candidate.Id);
            Assert.That(active.Status == EntityStatus.Active);

            candidateRepository.SetAsDeleted(candidate);
            var deleted = candidateRepository.GetById(candidate.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }

        private Candidate Create()
        {
            var candidateEntity = new Candidate(Guid.NewGuid())
            {
                FirstName = "John",
                MiddleName = "A",
                Surname = "Doe",
                Race = CreateRace().GetMasterDataRef(),
                PoliticalParty = CreatePoliticalParty().GetMasterDataRef(),
                IdCardNumber = "2568974".RandStr(),
                PassportNumber = "2568974".RandStr(),
                CandidateType = CandidateType.PartyBacked,
                EmailAddress = "abc@email.com",
                Status = EntityStatus.Active,
                RunningMateName = "Statham",
                RunningMateIdCardNumber = "2568974",
                CandidateStatus = CandidateStatus.Nominated,
            };
            return candidateEntity;
        }
    }
}