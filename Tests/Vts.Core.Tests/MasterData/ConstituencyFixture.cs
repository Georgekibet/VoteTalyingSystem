using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class ConstituencyFixture
    {
        [Test]
        public void Constituency_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            Constituency constituency = u.Create<Constituency>();
            //act
            ValidationResultInfo r = constituency.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}