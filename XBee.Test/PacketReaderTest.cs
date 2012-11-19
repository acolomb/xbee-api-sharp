using System.Collections;
using NUnit.Framework;
using XBee.Frames;

namespace XBee.Test
{
    [TestFixture]
    class PacketReaderTest
    {
        private Queue frames = new Queue();

        [Test]
        public void TestPacketReaderMultipleReceiveEvents()
        {
            var reader = new PacketReader();
            reader.FrameReceived += FrameReceivedEvent;
            
            var data = new byte[] {
                0x7E,
                0x00, 0x16,
                0x90, 
                0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 
                0x7D, 0x84, 
                0x01
            };
            var data2 = new byte[] {
                0x7E, 0x69, 0x6e, 0x63, 0x6c, 0x75, 0x64, 0x65, 0x20, 0x00, 0xCF
            };
            
            reader.ReceiveData(data);
            reader.ReceiveData(data2);
            
            Assert.That(frames.Count, Is.EqualTo(1));

            var frame = (XBeeFrame) frames.Dequeue();
            Assert.That(frame, Is.TypeOf<ZigBeeReceivePacket>());
        }

        [Test]
        public void TestPacketReaderMultiplePackets()
        {
            var reader = new PacketReader();
            reader.FrameReceived += FrameReceivedEvent;
            
            var data = new byte[] {
                0x7E, 0x00, 0x1B,
                0x88, 0x06,
                0x41, 0x53, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x55,
                0x80, 0x54,
                0x02, 0x10, 0x00, 0x08, 0x00,
                0xCF, 0xFF,
                0x00, 0x29,
                0xC5, 0xF2, 0x33,
                0xB8,
                0x7E, 0x00, 0x05,
                0x88, 0x06,
                0x41, 0x53, 0x00,
                0xDD
            };
            
            reader.ReceiveData(data);

            Assert.That(frames.Count, Is.EqualTo(2));

            var frame = (XBeeFrame) frames.Dequeue();
            Assert.That(frame, Is.TypeOf<ATCommandResponse>());
            Assert.That((frame as ATCommandResponse).Value, Is.Not.Null);

            frame = (XBeeFrame) frames.Dequeue();
            Assert.That(frame, Is.TypeOf<ATCommandResponse>());
            Assert.That((frame as ATCommandResponse).Value, Is.Null);
        }

        private void FrameReceivedEvent(object sender, FrameReceivedArgs args)
        {
            frames.Enqueue(args.Response);
        }
    }
}
