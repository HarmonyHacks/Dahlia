using NUnit.Framework;

namespace Dahlia.Tests
{
    [TestFixture]
    public class WithATestAssembly
    {
        [Test]
        public void TestTheTruth()
        {
            Assert.That(true, Is.True);
        }        
    }
}