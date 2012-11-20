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

        [Test]
        public void TestUnescapeData()
        {
            var data = new byte[]
            {
                0x7E, 0x00, 0x16, 0x10, 0x01, 0x00, 0x7D, 0x33, 0xA2, 0x00, 0x40, 0x0A, 0x01, 0x27, 0xFF,
                0xFE, 0x00, 0x00, 0x54, 0x78, 0x44, 0x61, 0x74, 0x61, 0x30, 0x41, 0x7D, 0x33
            };
            var expected = new byte[]
            {
                0x00, 0x16, 0x10, 0x01, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x0A, 0x01, 0x27, 0xFF, 0xFE, 0x00,
                0x00, 0x54, 0x78, 0x44, 0x61, 0x74, 0x61, 0x30, 0x41, 0x13
            };
            var result = XBeeEscapeCharacters.UnescapeData(data);
            Assert.That(result.ToArray(), Is.EqualTo(expected));
        }
    }
}
