using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class PollingCentreFixture
    {
        [Test]
        public void PollingCentre_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            PollingCentre pollingCentre = u.Create<PollingCentre>();
            //act
            ValidationResultInfo r = pollingCentre.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}