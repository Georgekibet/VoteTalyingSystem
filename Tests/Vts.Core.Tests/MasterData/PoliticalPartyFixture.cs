using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class PoliticalPartyFixture
    {
        [Test]
        public void PoliticalParty_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            PoliticalParty politicalParty = u.Create<PoliticalParty>();
            //act
            ValidationResultInfo r = politicalParty.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}