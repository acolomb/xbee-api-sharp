using NUnit.Framework;
using XBee;
using XBee.Frames;
using XBee.Frames.ATCommands;
using XBee.Exceptions;

namespace XBee.Test.Frames
{
    [TestFixture]
    public class ATCommandResponseTest
    {
        [Test]
        public void TestATCommandResponseParse()
        {
            var packet = new byte[] { 0x00, 0x05, 0x88, 0x01, 0x42, 0x44, 0x00, 0xF0 };

            var frame = XBeePacketUnmarshaler.Unmarshal(packet, ApiVersion.S1);
            Assert.That(frame, Is.InstanceOf<ATCommandResponse>());

            var cmd = (ATCommandResponse) frame;
            Assert.That(cmd.FrameId, Is.EqualTo(0x01));
            Assert.That(cmd.Command, Is.EqualTo(AT.BaudRate));
            Assert.That(cmd.CommandStatus, Is.EqualTo(ATCommandResponse.CommandStatusType.Ok));
        }

        [Test]
        [ExpectedException(typeof(XBeeFrameException), ExpectedMessage = "Unsupported ATNR command for specified API version S1.")]
        public void TestATCommandResponseWrongApiVersion()
        {
            var packet = new byte[] { 0x00, 0x05, 0x88, 0x01, 0x4E, 0x52, 0x00, 0xD6 };
            XBeePacketUnmarshaler.Unmarshal(packet, ApiVersion.S1);
        }

        [Test]
        public void TestNetworkDiscoveryParsing()
        {
            var packet = new byte[]
                {
                    0x00, 0x19, 0x88, 0x01, 0x4E, 0x44, 0x00, 0x00, 0x00, 0x00, 0x13, 0xA2, 0x00,
                    0x40, 0x47, 0x81, 0x4F, 0x20, 0x00, 0xFF, 0xFE, 0x00, 0x00, 0xC1, 0x05, 0x10,
                    0x1E, 0xC7
                };

            var frame = XBeePacketUnmarshaler.Unmarshal(packet, ApiVersion.S2);
            Assert.That(frame, Is.InstanceOf<ATCommandResponse>());
        }
    }
}
