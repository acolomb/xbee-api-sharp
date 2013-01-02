using NUnit.Framework;
using XBee.Frames;
using XBee.Exceptions;

namespace XBee.Test
{
    [TestFixture]
    class XBeeEscapedPacketTest
    {
        [Test]
        public void TestXBeeEscapedPacketSequence()
        {
            var data = new byte[]
            {
                0x10, 0x01,             //CommandID, FrameID
                0x00, 0x13, 0xA2, 0x00, 0x40, 0x0A, 0x01, 0x27,         //Destination
                0xFF, 0xFE,             //Destination Network Address
                0x00, 0x00,             //Radius, Options
                0x7D, 0x7E, 0xBC        //RF Data
            };
            var expected = new byte[]
            {
                0x7E, 0x00, 0x7D, 0x31, //Start, Length (escaped)
                0x10, 0x01,             //CommandID, FrameID
                0x00, 0x7D, 0x33, 0xA2, 0x00, 0x40, 0x0A, 0x01, 0x27,   //Destination (escaped)
                0xFF, 0xFE,             //Destination Network Address
                0x00, 0x00,             //Radius, Options
                0x7D, 0x5D, 0x7D, 0x5E, 0xBC,   //RF Data (escaped)
                0x7D, 0x33              //Checksum (escaped)
            };

            var packet = new XBeeEscapedPacket(data);
            packet.Assemble();

            Assert.That(packet.Data, Is.EqualTo(expected));
        }
    }
}
