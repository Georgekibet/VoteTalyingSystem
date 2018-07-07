using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class WardFixture
    {
        [Test]
        public void Ward_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            Ward ward = u.Create<Ward>();
            //act
            ValidationResultInfo r = ward.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}