using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class CountyFixture
    {
        [Test]
        public void County_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            County county = u.Create<County>();
            //act
            ValidationResultInfo r = county.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}