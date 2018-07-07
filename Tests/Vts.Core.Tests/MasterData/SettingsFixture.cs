using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class SettingsFixture
    {
        [Test]
        public void Settings_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            Settings settings = u.Create<Settings>();
            //act
            ValidationResultInfo r = settings.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}