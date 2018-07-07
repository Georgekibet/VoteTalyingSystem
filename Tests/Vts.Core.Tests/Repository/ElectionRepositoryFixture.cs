using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using vts.Core.Shared.Entities.Master;
using vts.Data.Repository;
using vts.Data.Repository.MasterData;
using vts.Shared.Entities.Master;

namespace Vts.Core.Tests.Repository
{
    [TestFixture, Category("VtsIntegrationTests"), Category("IT_RepositoryFixtures")]
    internal class ElectionRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_Election()
        {
            var election = Create();
            var electionRepository = new ElectionRepository(ContextConnection());
            var id = electionRepository.Save(election);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, election.Id);
        }

        [Test]
        public void SimpeDeHydrate_Election()
        {
            var election = Create();
            var electionRepository = new ElectionRepository(ContextConnection());
            var id = electionRepository.Save(election);
            var owner = electionRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, election.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_Election()
        {
            var electionRepository = new ElectionRepository(ContextConnection());
            var election = Create();
            electionRepository.Save(election);
            var owner = electionRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_Election()
        {
            var electionRepository = new ElectionRepository(ContextConnection());
            var election = Create();
            electionRepository.Save(election);
            electionRepository.SetInactive(election);
            var inactive = electionRepository.GetById(election.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            electionRepository.SetActive(election);
            var active = electionRepository.GetById(election.Id);
            Assert.That(active.Status == EntityStatus.Active);

            electionRepository.SetAsDeleted(election);
            var deleted = electionRepository.GetById(election.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }

        private Election Create()
        {
            var electionEntity = new Election(Guid.NewGuid())
            {
                Name = "Kenya".RandStr(),
                Date = DateTime.Now.AddMonths(2),
                NominationStartDate = DateTime.Now.AddMonths(-12),
                NominationEndDate = DateTime.Now.AddMonths(-11),
                ElectionType = ElectionType.GeneralElection,
                Location = "0722000000",
                Status = EntityStatus.Active
            };
            return electionEntity;
        }
    }
}