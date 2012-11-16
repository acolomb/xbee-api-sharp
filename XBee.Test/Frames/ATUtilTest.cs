using NUnit.Framework;
using XBee.Frames;
using XBee.Frames.ATCommands;

namespace XBee.Test.Frames
{
    [TestFixture]
    class ATUtilTest
    {
        [Test]
        public void ATUtilValid()
        {
            Assert.That(ATUtil.Parse("DH"), Is.EqualTo(AT.DestinationHigh));
        }

        [Test]
        public void ATUtilDefaultReturnsUnknown()
        {
            AT at = default(AT);
            Assert.That(at, Is.EqualTo(AT.Unknown));
        }
        
        [Test]
        public void ATUtilInvalidReturnsUnknown()
        {
            Assert.That(ATUtil.Parse("11"), Is.EqualTo(AT.Unknown));
        }

        [Test]
        public void ATUtilNullReturnsUnknown()
        {
            Assert.That(ATUtil.Parse(null), Is.EqualTo(AT.Unknown));
        }
    }
}
