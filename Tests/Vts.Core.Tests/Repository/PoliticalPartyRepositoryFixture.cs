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
    internal class PoliticalPartyRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_PoliticalParty()
        {
            var politicalParty = Create();
            var politicalPartyRepository = new PoliticalPartyRepository(ContextConnection());
            var id = politicalPartyRepository.Save(politicalParty);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, politicalParty.Id);
        }

        [Test]
        public void SimpeDeHydrate_PoliticalParty()
        {
            var politicalParty = Create();
            var politicalPartyRepository = new PoliticalPartyRepository(ContextConnection());
            var id = politicalPartyRepository.Save(politicalParty);
            var owner = politicalPartyRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, politicalParty.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_PoliticalParty()
        {
            var politicalPartyRepository = new PoliticalPartyRepository(ContextConnection());
            var politicalParty = Create();
            politicalPartyRepository.Save(politicalParty);
            var owner = politicalPartyRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_PoliticalParty()
        {
            var politicalPartyRepository = new PoliticalPartyRepository(ContextConnection());
            var politicalParty = Create();
            politicalPartyRepository.Save(politicalParty);
            politicalPartyRepository.SetInactive(politicalParty);
            var inactive = politicalPartyRepository.GetById(politicalParty.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            politicalPartyRepository.SetActive(politicalParty);
            var active = politicalPartyRepository.GetById(politicalParty.Id);
            Assert.That(active.Status == EntityStatus.Active);

            politicalPartyRepository.SetAsDeleted(politicalParty);
            var deleted = politicalPartyRepository.GetById(politicalParty.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }

        private PoliticalParty Create()
        {
            var politicalPartyEntity = new PoliticalParty(Guid.NewGuid())
            {
                Name = "Democratic".RandStr(),
                Code = "0011",
                Acronym = "DMC",
                DateRegistered = DateTime.Now.AddYears(-2),
                Status = EntityStatus.Active
            };
            return politicalPartyEntity;
        }
    }
}