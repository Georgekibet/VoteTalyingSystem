using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class UserFixture
    {
        [Test]
        public void User_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            User user = u.Create<User>();
            //act
            ValidationResultInfo r = user.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}