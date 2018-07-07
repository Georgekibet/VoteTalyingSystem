using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class RegionFixture
    {
        [Test]
        public void Region_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            Region region = u.Create<Region>();
            //act
            ValidationResultInfo r = region.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}