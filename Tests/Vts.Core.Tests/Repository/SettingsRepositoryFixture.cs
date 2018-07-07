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
    internal class SettingsRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_Settings()
        {
            var settings = Create();
            var settingsRepository = new SettingsRepository(ContextConnection());
            var id = settingsRepository.Save(settings);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, settings.Id);
        }

        [Test]
        public void SimpeDeHydrate_Settings()
        {
            var settings = Create();
            var settingsRepository = new SettingsRepository(ContextConnection());
            var id = settingsRepository.Save(settings);
            var owner = settingsRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, settings.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_Settings()
        {
            var settingsRepository = new SettingsRepository(ContextConnection());
            var settings = Create();
            settingsRepository.Save(settings);
            var owner = settingsRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_Settings()
        {
            var settingsRepository = new SettingsRepository(ContextConnection());
            var settings = Create();
            settingsRepository.Save(settings);
            settingsRepository.SetInactive(settings);
            var inactive = settingsRepository.GetById(settings.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            settingsRepository.SetActive(settings);
            var active = settingsRepository.GetById(settings.Id);
            Assert.That(active.Status == EntityStatus.Active);

            settingsRepository.SetAsDeleted(settings);
            var deleted = settingsRepository.GetById(settings.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }

        private Settings Create()
        {
            Random rnd = new Random();
            int key = rnd.Next(1, 100); 
            var settingsEntity = new Settings(Guid.NewGuid())
            {
                Key = (SettingsKeys)key,
                Value = "John",
                Status = EntityStatus.Active
            };
            return settingsEntity;
        }
    }
}