using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using vts.Core.Shared.Entities.Master;
using vts.Data.Repository;
using vts.Data.Repository.MasterData;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;

namespace Vts.Core.Tests.Repository
{
    [TestFixture, Category("VtsIntegrationTests"), Category("IT_RepositoryFixtures")]
    internal class WardRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_Ward()
        {
            IConstituencyRepository constituencyRepository = Substitute.For<IConstituencyRepository>();
            var f = new Fixture();
            var constituency = f.Create<Constituency>();
            var ward = CreateWard(constituency.GetMasterDataRef());
            constituencyRepository.GetById(Arg.Any<Guid>()).Returns(constituency);
            var wardRepository = new WardRepository(ContextConnection(), constituencyRepository);
            var id = wardRepository.Save(ward);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, ward.Id);
        }

        [Test]
        public void SimpeDeHydrate_Ward()
        {
            IConstituencyRepository constituencyRepository = Substitute.For<IConstituencyRepository>();
            var f = new Fixture();
            var constituency = f.Create<Constituency>();
            var ward = CreateWard(constituency.GetMasterDataRef());
            constituencyRepository.GetById(Arg.Any<Guid>()).Returns(constituency);
            var wardRepository = new WardRepository(ContextConnection(), constituencyRepository);
            var id = wardRepository.Save(ward);
            var owner = wardRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, ward.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_Ward()
        {
            IConstituencyRepository constituencyRepository = Substitute.For<IConstituencyRepository>();
            var f = new Fixture();
            var constituency = f.Create<Constituency>();
            var ward = CreateWard(constituency.GetMasterDataRef());
            constituencyRepository.GetById(Arg.Any<Guid>()).Returns(constituency);
            var wardRepository = new WardRepository(ContextConnection(), constituencyRepository);
            wardRepository.Save(ward);
            var owner = wardRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_Ward()
        {
            IConstituencyRepository constituencyRepository = Substitute.For<IConstituencyRepository>();
            var f = new Fixture();
            var constituency = f.Create<Constituency>();
            var ward = CreateWard(constituency.GetMasterDataRef());
            constituencyRepository.GetById(Arg.Any<Guid>()).Returns(constituency);
            var wardRepository = new WardRepository(ContextConnection(), constituencyRepository);
            wardRepository.Save(ward);
            wardRepository.SetInactive(ward);
            var inactive = wardRepository.GetById(ward.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            wardRepository.SetActive(ward);
            var active = wardRepository.GetById(ward.Id);
            Assert.That(active.Status == EntityStatus.Active);

            wardRepository.SetAsDeleted(ward);
            var deleted = wardRepository.GetById(ward.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }
    }
}