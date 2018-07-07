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
    internal class PollingCentreRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_PollingCentre()
        {
            IWardRepository wardRepository = Substitute.For<IWardRepository>();
            var f = new Fixture();
            var ward = f.Create<Ward>();
            var pollingCentre = Create(ward.GetMasterDataRef());
            wardRepository.GetById(Arg.Any<Guid>()).Returns(ward);
            var pollingCentreRepository = new PollingCentreRepository(ContextConnection(), wardRepository);
            var id = pollingCentreRepository.Save(pollingCentre);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, pollingCentre.Id);
        }

        [Test]
        public void SimpeDeHydrate_PollingCentre()
        {
            IWardRepository wardRepository = Substitute.For<IWardRepository>();
            var f = new Fixture();
            var ward = f.Create<Ward>();
            var pollingCentre = Create(ward.GetMasterDataRef());
            wardRepository.GetById(Arg.Any<Guid>()).Returns(ward);
            var pollingCentreRepository = new PollingCentreRepository(ContextConnection(), wardRepository);
            var id = pollingCentreRepository.Save(pollingCentre);
            var owner = pollingCentreRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, pollingCentre.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_PollingCentre()
        {
            IWardRepository wardRepository = Substitute.For<IWardRepository>();
            var f = new Fixture();
            var ward = f.Create<Ward>();
            var pollingCentre = Create(ward.GetMasterDataRef());
            wardRepository.GetById(Arg.Any<Guid>()).Returns(ward);
            var pollingCentreRepository = new PollingCentreRepository(ContextConnection(), wardRepository);
            pollingCentreRepository.Save(pollingCentre);
            var owner = pollingCentreRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_PollingCentre()
        {
            IWardRepository wardRepository = Substitute.For<IWardRepository>();
            var f = new Fixture();
            var ward = f.Create<Ward>();
            var pollingCentre = Create(ward.GetMasterDataRef());
            wardRepository.GetById(Arg.Any<Guid>()).Returns(ward);
            var pollingCentreRepository = new PollingCentreRepository(ContextConnection(), wardRepository);
            pollingCentreRepository.Save(pollingCentre);
            pollingCentreRepository.SetInactive(pollingCentre);
            var inactive = pollingCentreRepository.GetById(pollingCentre.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            pollingCentreRepository.SetActive(pollingCentre);
            var active = pollingCentreRepository.GetById(pollingCentre.Id);
            Assert.That(active.Status == EntityStatus.Active);

            pollingCentreRepository.SetAsDeleted(pollingCentre);
            var deleted = pollingCentreRepository.GetById(pollingCentre.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }

        private PollingCentre Create(WardRef ward)
        {
            var pollingCentreEntity = new PollingCentre(Guid.NewGuid())
            {
                Name = "Democratic".RandStr(),
                Code = "0011",
                Ward = ward,
                RegisteredVoters = 7000,
                Streams = 25,
                PollingCentreType = PollingCentreType.None,
                Status = EntityStatus.Active
            };
            return pollingCentreEntity;
        }
    }
}