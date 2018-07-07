using NUnit.Framework;
using Ploeh.AutoFixture;
using vts.Shared.Entities.Master;
using vts.Shared.Services;

namespace Vts.Core.Tests.MasterData
{
    [TestFixture, Category("vts.Core.Test.MasterData")]
    public class CandidateFixture
    {
        [Test]
        public void Candidate_withValid_validations_shouldPass()
        {
            //arrange
            var u = new Fixture();
            Candidate candidate = u.Create<Candidate>();
            //act
            ValidationResultInfo r = candidate.Validate();
            //assert
            Assert.That(r.IsValid, Is.True, "Validation should pass");
        }
    }
}