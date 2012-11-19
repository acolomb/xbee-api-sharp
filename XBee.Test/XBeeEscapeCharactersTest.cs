using NUnit.Framework;
using XBee.Frames;

namespace XBee.Test
{
    [TestFixture]
    class XBeeEscapeCharactersTest
    {
        [Test]
        public void TestIsSpecialByteIsTrue()
        {
            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0x7E), Is.True);
            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0x7D), Is.True);
            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0x11), Is.True);
            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0x13), Is.True);
        }

        [Test]
        public void TestIsSpecialByteIsFalse()
        {
            var reader = new EscapedPacketReader();

            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0x77), Is.False);
            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0xEE), Is.False);
            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0x1D), Is.False);
            Assert.That(XBeeEscapeCharacters.IsSpecialByte(0xD2), Is.False);
        }
    }
}
