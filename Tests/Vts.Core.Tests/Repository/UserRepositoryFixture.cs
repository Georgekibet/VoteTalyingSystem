using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using vts.Core.Shared.Entities.Master;
using vts.Data.Repository.MasterData;

namespace Vts.Core.Tests.Repository
{
    [TestFixture, Category("VtsIntegrationTests"), Category("IT_RepositoryFixtures")]
    internal class UserRepositoryFixture : BaseFixture
    {
        [Test]
        public void SimpeHydrate_User()
        {
            var user = Create();
            var userRepository = new UserRepository(ContextConnection());
            var id = userRepository.Save(user);
            Assert.IsNotNull(id);
            Assert.AreEqual(id, user.Id);
        }

        [Test]
        public void SimpeDeHydrate_User()
        {
            var user = Create();
            var userRepository = new UserRepository(ContextConnection());
            var id = userRepository.Save(user);
            var owner = userRepository.GetById(id);
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Id, user.Id);
        }

        [Test]
        public void SimpeDeHydrateAll_User()
        {
            var userRepository = new UserRepository(ContextConnection());
            var user = Create();
            userRepository.Save(user);
            var owner = userRepository.GetAll();
            Assert.That(owner.Any());
        }

        [Test]
        public void Simple_Status_User()
        {
            var userRepository = new UserRepository(ContextConnection());
            var user = Create();
            userRepository.Save(user);
            userRepository.SetInactive(user);
            var inactive = userRepository.GetById(user.Id);
            Assert.That(inactive.Status == EntityStatus.Inactive);

            userRepository.SetActive(user);
            var active = userRepository.GetById(user.Id);
            Assert.That(active.Status == EntityStatus.Active);

            userRepository.SetAsDeleted(user);
            var deleted = userRepository.GetById(user.Id);
            Assert.That(deleted.Status == EntityStatus.Deleted);
        }

        private User Create()
        {
            var userEntity = new User(Guid.NewGuid())
            {
                Username = "annonymous".RandStr(),
                FirstName = "John",
                MiddleName = "A",
                Surname = "Doe",
                Password = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes("supersecret"))),
                UserType = UserType.Admin,
                Mobile = "0722000000",
                Status = EntityStatus.Active
            };
            return userEntity;
        }
    }
}