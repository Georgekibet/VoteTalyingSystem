using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class RaceFixture
    {
        [Test]
        public void Race_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            Race race = u.Create<Race>();
            //act
            ValidationResultInfo r = race.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}