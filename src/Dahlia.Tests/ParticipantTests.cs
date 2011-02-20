using Dahlia.Models;
using NUnit.Framework;

namespace Dahlia.Tests
{
    [TestFixture]
    public class ParticipantTests
    {
        [Test]
        public void Two_Participants_with_the_same_ids_are_equal()
        {
            var p1 = new Participant { Id = 1 };
            var p2 = new Participant { Id = 1 };

            Assert.That(p1.Equals(p2));
            Assert.That(p2.Equals(p1));
        }

        [Test]
        public void Two_Participants_with_the_different_ids_are_not_equal()
        {
            var p1 = new Participant { Id = 1 };
            var p2 = new Participant { Id = 2 };

            Assert.That(p1.Equals(p2), Is.False);
            Assert.That(p2.Equals(p1), Is.False);
        }
    }
}