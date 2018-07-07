using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class ElectionFixture
    {
        [Test]
        public void Election_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            Election election = u.Create<Election>();
            //act
            ValidationResultInfo r = election.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}