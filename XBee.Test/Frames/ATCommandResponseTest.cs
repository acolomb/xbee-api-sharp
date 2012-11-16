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
                    0x00, 0x15, 0x88, 0x01, 0x4E, 0x44, 0x00, 0x12, 0x34, 0x00, 0x13, 0xA2, 0x00,
                    0x40, 0x47, 0x81, 0x4F, 0x20, 0x54, 0x45, 0x53, 0x54, 0x00, 0x32
                };

            var frame = XBeePacketUnmarshaler.Unmarshal(packet, ApiVersion.S1);
            Assert.That(frame, Is.InstanceOf<ATCommandResponse>());

            var cmd = (ATCommandResponse) frame;
            Assert.That(cmd.ExpectedApi, Is.EqualTo(ApiVersion.S1));
            Assert.That(cmd.FrameId, Is.EqualTo(0x01));
            Assert.That(cmd.Command, Is.EqualTo(AT.NodeDiscover));
            Assert.That(cmd.CommandStatus, Is.EqualTo(ATCommandResponse.CommandStatusType.Ok));
            Assert.That(cmd.Value, Is.InstanceOf<ATNodeDiscoverValue>());

            var value = (ATNodeDiscoverValue) cmd.Value;
            Assert.That(value.Source.Address16.Equals(new XBeeAddress16(0x1234)), Is.True);
            Assert.That(value.Source.Address64.Equals(new XBeeAddress64(0x0013A2004047814F)), Is.True);
            Assert.That(value.SignalStrength.Value, Is.EqualTo(32));
            Assert.That(value.NodeIdentifier.Value, Is.EqualTo("TEST"));
        }

        [Test]
        public void TestNetworkDiscoveryZBParsing()
        {
            var packet = new byte[]
            {
                0x00, 0x1A, 0x88, 0x01, 0x4E, 0x44, 0x00, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x47, 
                0x81, 0x4F, 0x54, 0x45, 0x53, 0x54, 0x00, 0xFF, 0xFE, 0x00, 0x00, 0xC1, 0x05,
                0x10, 0x1E, 0xA7
            };
            
            var frame = XBeePacketUnmarshaler.Unmarshal(packet, ApiVersion.S2);
            Assert.That(frame, Is.InstanceOf<ATCommandResponse>());
            
            var cmd = (ATCommandResponse) frame;
            Assert.That(cmd.ExpectedApi, Is.EqualTo(ApiVersion.S2));
            Assert.That(cmd.FrameId, Is.EqualTo(0x01));
            Assert.That(cmd.Command, Is.EqualTo(AT.NodeDiscoverZB));
            Assert.That(cmd.CommandStatus, Is.EqualTo(ATCommandResponse.CommandStatusType.Ok));
            Assert.That(cmd.Value, Is.InstanceOf<ATNodeDiscoverZBValue>());
            
            var value = (ATNodeDiscoverZBValue) cmd.Value;
            Assert.That(value.Source.Address64.Equals(new XBeeAddress64(0x0013A2004047814F)), Is.True);
            Assert.That(value.NodeIdentifier.Value, Is.EqualTo("TEST"));
            Assert.That(value.ParentAddress.Equals(new XBeeAddress16(0xFFFE)), Is.True);
            Assert.That(value.DeviceType, Is.EqualTo(NodeIdentification.DeviceType.Coordinator));
            Assert.That(value.Status, Is.EqualTo(0));
            Assert.That(value.ProfileId.Value, Is.EqualTo(0xC105));
            Assert.That(value.ManufacturerId.Value, Is.EqualTo(0x101E));
        }
    }
}
