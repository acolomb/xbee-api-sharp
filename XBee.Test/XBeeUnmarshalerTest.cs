using System;
using NUnit.Framework;
using XBee.Frames;
using XBee.Frames.ATCommands;
using XBee.Exceptions;

namespace XBee.Test
{
    [TestFixture]
    class XBeeUnmarshalerTest
    {

        public class NotXBeeFrame
        {
            public NotXBeeFrame() { }
        }

        public class XBeeUnknownFrame : XBeeFrame
        {
            public XBeeUnknownFrame() { }

            public override byte[] ToByteArray()
            {
                return new byte[] { };
            }

            public override void Parse()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        [ExpectedException(typeof(XBeeException), ExpectedMessage = "Invalid Frame Handler")]
        public void TestXBeeUnmarshalerRegisterWrong()
        {
            XBeePacketUnmarshaler.RegisterFrameHandler(XBeeAPICommandId.REMOTE_AT_REQUEST, typeof(NotXBeeFrame));
        }

        [Test]
        public void TestXBeeUnmarshalerRegister()
        {
            XBeePacketUnmarshaler.RegisterFrameHandler(XBeeAPICommandId.UNKNOWN, typeof(XBeeUnknownFrame));
        }

        [Test]
        public void TestXBeeUnmarshalerATCommandWrongLength()
        {
            var packetData = new byte[] { 0x00, 0x08, 0x08, 0x01, (byte) 'D', (byte) 'H' };
            Assert.Throws<XBeeFrameException>(delegate { XBeeFrame frame = XBeePacketUnmarshaler.Unmarshal(packetData, ApiVersion.S1); });
        }

        [Test]
        public void TestXBeeUnmarshalerATCommandNoData()
        {
            var packetData = new byte[] { 0x00, 0x04, 0x08, 0x01, (byte) 'D', (byte) 'H', 0x6A };

            var frame = XBeePacketUnmarshaler.Unmarshal(packetData, ApiVersion.S1);
            Assert.That(frame, Is.InstanceOf<ATCommand>());
            var cmd = (ATCommand) frame;
            Assert.That(cmd.FrameId, Is.EqualTo(0x01));
            Assert.That(cmd.Command, Is.EqualTo(AT.DestinationHigh));
        }

        [Test]
        public void TestXBeeUnmarshalerATCommand()
        {
            var packetData = new byte[] { 0x00, 0x08, 0x08, 0x01, (byte) 'D', (byte) 'H', 0x11, 0x22, 0x33, 0x00, 0x04 };

            var frame = XBeePacketUnmarshaler.Unmarshal(packetData, ApiVersion.S1);
            Assert.That(frame, Is.InstanceOf<ATCommand>());
            var cmd = (ATCommand) frame;
            Assert.That(cmd.FrameId, Is.EqualTo(0x01));
            Assert.That(cmd.Command, Is.EqualTo(AT.DestinationHigh));
        }

        [Test]
        public void TestXBeeUnmarshalerWrongFrameApiVersion()
        {
            var packetData = new byte[] { 0x00, 0x12, 0x90, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x7D, 0x84, 0x01, 0x52, 0x78, 0x44, 0x61, 0x74, 0x61, 0x0D };
            Assert.Throws<XBeeFrameException>(delegate { XBeeFrame frame = XBeePacketUnmarshaler.Unmarshal(packetData, ApiVersion.S1); });
        }
    }
}
