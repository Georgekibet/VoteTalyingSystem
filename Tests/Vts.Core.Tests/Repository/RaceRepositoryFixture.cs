using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Core.Shared.Entities.Master;
using vts.Data.Repository;
using vts.Data.Repository.MasterData;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;

namespace Vts.Core.Tests.Repository
{
    [TestFixture, Category("VtsIntegrationTests"), Category("IT_RepositoryFixtures")]
    internal class RaceRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_Race()
        {
            IElectionRepository electionRepository = Substitute.For<IElectionRepository>();
            var f = new Fixture();
            var election = f.Create<Election>();
            var race = Create(election.GetMasterDataRef());
            electionRepository.GetById(Arg.Any<Guid>()).Returns(election);
            var raceRepository = new RaceRepository(ContextConnection(), electionRepository);
            var id = raceRepository.Save(race);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, race.Id);
        }

        [Test]
        public void SimpeDeHydrate_Race()
        {
            IElectionRepository electionRepository = Substitute.For<IElectionRepository>();
            var f = new Fixture();
            var election = f.Create<Election>();
            var race = Create(election.GetMasterDataRef());
            electionRepository.GetById(Arg.Any<Guid>()).Returns(election);
            var raceRepository = new RaceRepository(ContextConnection(), electionRepository);
            var id = raceRepository.Save(race);
            var owner = raceRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, race.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_Race()
        {
            IElectionRepository electionRepository = Substitute.For<IElectionRepository>();
            var f = new Fixture();
            var election = f.Create<Election>();
            var race = Create(election.GetMasterDataRef());
            electionRepository.GetById(Arg.Any<Guid>()).Returns(election);
            var raceRepository = new RaceRepository(ContextConnection(), electionRepository);
            raceRepository.Save(race);
            var owner = raceRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_Race()
        {
            IElectionRepository electionRepository = Substitute.For<IElectionRepository>();
            var f = new Fixture();
            var election = f.Create<Election>();
            var race = Create(election.GetMasterDataRef());
            electionRepository.GetById(Arg.Any<Guid>()).Returns(election);
            var raceRepository = new RaceRepository(ContextConnection(), electionRepository);
            raceRepository.Save(race);
            raceRepository.SetInactive(race);
            var inactive = raceRepository.GetById(race.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            raceRepository.SetActive(race);
            var active = raceRepository.GetById(race.Id);
            Assert.That(active.Status == EntityStatus.Active);

            raceRepository.SetAsDeleted(race);
            var deleted = raceRepository.GetById(race.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }

        private Race Create(ElectionRef election)
        {
            var raceEntity = new Race(Guid.NewGuid())
            {
                Name = "Democratic".RandStr(),
                RaceType = RaceType.Senator,
                Election = election,
                Status = EntityStatus.Active
            };
            return raceEntity;
        }
    }
}
