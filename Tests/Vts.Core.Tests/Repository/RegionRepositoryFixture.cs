using NUnit.Framework;
using System.Linq;
using vts.Core.Shared.Entities.Master;
using vts.Data.Repository;
using vts.Data.Repository.MasterData;

namespace Vts.Core.Tests.Repository
{
    [TestFixture, Category("VtsIntegrationTests"), Category("IT_RepositoryFixtures")]
    internal class RegionRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_Region()
        {
            var region = CreateRegion();
            var regionRepository = new RegionRepository(ContextConnection());
            var id = regionRepository.Save(region);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, region.Id);
        }

        [Test]
        public void SimpeDeHydrate_Region()
        {
            var region = CreateRegion();
            var regionRepository = new RegionRepository(ContextConnection());
            var id = regionRepository.Save(region);
            var owner = regionRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, region.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_Region()
        {
            var regionRepository = new RegionRepository(ContextConnection());
            var region = CreateRegion();
            regionRepository.Save(region);
            var owner = regionRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_Region()
        {
            var regionRepository = new RegionRepository(ContextConnection());
            var region = CreateRegion();
            regionRepository.Save(region);
            regionRepository.SetInactive(region);
            var inactive = regionRepository.GetById(region.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            regionRepository.SetActive(region);
            var active = regionRepository.GetById(region.Id);
            Assert.That(active.Status == EntityStatus.Active);

            regionRepository.SetAsDeleted(region);
            var deleted = regionRepository.GetById(region.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }
    }
}