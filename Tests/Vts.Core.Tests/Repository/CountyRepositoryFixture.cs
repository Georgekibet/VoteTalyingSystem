using System;
using NUnit.Framework;
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
    internal class CountyRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_County()
        {
            IRegionRepository regionRepository = Substitute.For<IRegionRepository>();
            var f = new Fixture();
            var region = f.Create<Region>();
            var county = CreateCounty(region.GetMasterDataRef());
            regionRepository.GetById(Arg.Any<Guid>()).Returns(region);
            var countyRepository = new CountyRepository(ContextConnection(),regionRepository);
            var id = countyRepository.Save(county);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, county.Id);
        }

        [Test]
        public void SimpeDeHydrate_County()
        {
            IRegionRepository regionRepository = Substitute.For<IRegionRepository>();
            var f = new Fixture();
            var region = f.Create<Region>();
            var county = CreateCounty(region.GetMasterDataRef());
            regionRepository.GetById(Arg.Any<Guid>()).Returns(region);
            var countyRepository = new CountyRepository(ContextConnection(), regionRepository);
            var id = countyRepository.Save(county);
            var owner = countyRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, county.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_County()
        {
            IRegionRepository regionRepository = Substitute.For<IRegionRepository>();
            var f = new Fixture();
            var region = f.Create<Region>();
            var county = CreateCounty(region.GetMasterDataRef());
            regionRepository.GetById(Arg.Any<Guid>()).Returns(region);
            var countyRepository = new CountyRepository(ContextConnection(), regionRepository);
            countyRepository.Save(county);
            var owner = countyRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_County()
        {
            IRegionRepository regionRepository = Substitute.For<IRegionRepository>();
            var f = new Fixture();
            var region = f.Create<Region>();
            var county = CreateCounty(region.GetMasterDataRef());
            regionRepository.GetById(Arg.Any<Guid>()).Returns(region);
            var countyRepository = new CountyRepository(ContextConnection(), regionRepository);
            countyRepository.Save(county);
            countyRepository.SetInactive(county);
            var inactive = countyRepository.GetById(county.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            countyRepository.SetActive(county);
            var active = countyRepository.GetById(county.Id);
            Assert.That(active.Status == EntityStatus.Active);

            countyRepository.SetAsDeleted(county);
            var deleted = countyRepository.GetById(county.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }
    }
}