using NUnit.Framework;
using XBee.Frames;

namespace XBee.Test
{
    [TestFixture]
    class EscapedPacketReaderTest
    {
        private XBeeFrame frame;

        [Test]
        public void TestReceiveData()
        {
            var reader = new EscapedPacketReader();
            reader.FrameReceived += FrameReceivedEvent;
            var data = new byte[] { 0x7E, 0x00, 0x06, 0x88, 0x01, 0x41, 0x50, 0x00, 0x01, 0xE4 };
            frame = null;
            reader.ReceiveData(data);
            Assert.That(frame, Is.Not.Null);
            Assert.That(frame, Is.TypeOf<ATCommandResponse>());
        }

        [Test]
        public void TestReceiveEscapedData()
        {
            var data = new byte[] {
                0x7E, 0x00, 0x16, 0x90, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA,
                0x7D, 0x5D, 0x84, 0x01, 0x7D, 0x5E, 0x69, 0x6e, 0x63, 0x6c, 0x75, 0x64, 0x65, 0x20, 0x00, 0xCF
            };

            var reader = new EscapedPacketReader();
            frame = null;
            reader.FrameReceived += FrameReceivedEvent;

            reader.ReceiveData(data);
            Assert.That(frame, Is.Not.Null);
            Assert.That(frame, Is.TypeOf<ZigBeeReceivePacket>());
        }

        private void FrameReceivedEvent(object sender, FrameReceivedEventArgs args)
        {
            frame = args.Response;
        }
    }
}
