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
    internal class ConstituencyRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_Constituency()
        {
            ICountyRepository countyRepository = Substitute.For<ICountyRepository>();
            var f = new Fixture();
            var county = f.Create<County>();
            var constituency = CreateConstituency(county.GetMasterDataRef());
            countyRepository.GetById(Arg.Any<Guid>()).Returns(county);
            var constituencyRepository = new ConstituencyRepository(ContextConnection(),countyRepository);
            var id = constituencyRepository.Save(constituency);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, constituency.Id);
        }

        [Test]
        public void SimpeDeHydrate_Constituency()
        {
            ICountyRepository countyRepository = Substitute.For<ICountyRepository>();
            var f = new Fixture();
            var county = f.Create<County>();
            var constituency = CreateConstituency(county.GetMasterDataRef());
            countyRepository.GetById(Arg.Any<Guid>()).Returns(county);
            var constituencyRepository = new ConstituencyRepository(ContextConnection(), countyRepository);
            var id = constituencyRepository.Save(constituency);
            var owner = constituencyRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, constituency.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_Constituency()
        {
            ICountyRepository countyRepository = Substitute.For<ICountyRepository>();
            var f = new Fixture();
            var county = f.Create<County>();
            var constituency = CreateConstituency(county.GetMasterDataRef());
            countyRepository.GetById(Arg.Any<Guid>()).Returns(county);
            var constituencyRepository = new ConstituencyRepository(ContextConnection(), countyRepository);
            constituencyRepository.Save(constituency);
            var owner = constituencyRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_Constituency()
        {
            ICountyRepository countyRepository = Substitute.For<ICountyRepository>();
            var f = new Fixture();
            var county = f.Create<County>();
            var constituency = CreateConstituency(county.GetMasterDataRef());
            countyRepository.GetById(Arg.Any<Guid>()).Returns(county);
            var constituencyRepository = new ConstituencyRepository(ContextConnection(), countyRepository);
            constituencyRepository.Save(constituency);
            constituencyRepository.SetInactive(constituency);
            var inactive = constituencyRepository.GetById(constituency.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            constituencyRepository.SetActive(constituency);
            var active = constituencyRepository.GetById(constituency.Id);
            Assert.That(active.Status == EntityStatus.Active);

            constituencyRepository.SetAsDeleted(constituency);
            var deleted = constituencyRepository.GetById(constituency.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }
    }
}