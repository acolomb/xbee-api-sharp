﻿using System;
using System.Text;
using XBee.Frames.ATCommands;
using XBee.Utils;

namespace XBee.Frames
{
    public class ATCommandResponse : XBeeFrame
    {
        public enum CommandStatusType : byte
        {
            Ok = 0x00,
            Error = 0x01,
            InvalidCommand = 0x02,
            InvalidParameter = 0x03,
            TransmissionFailed = 0x04
        }
        
        protected readonly PacketParser parser;

        public AT Command { get; protected set; }
        public ApiVersion ExpectedApi { get; set; }
        public ATValue Value { get; protected set; }
        public CommandStatusType CommandStatus { get; protected set; }

        public ATCommandResponse(PacketParser parser)
        {
            this.parser = parser;
            CommandId = XBeeAPICommandId.AT_COMMAND_RESPONSE;
        }

        public ATCommandResponse()
        {
            CommandId = XBeeAPICommandId.AT_COMMAND_RESPONSE;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { };
        }

        public override void UseApiVersion(ApiVersion requested)
        {
            base.UseApiVersion(requested);
            ExpectedApi = requested;
        }

        public override void Parse()
        {
            FrameId = (byte)parser.ReadByte();
            Command = parser.ReadATCommand(ExpectedApi);
            CommandStatus = (CommandStatusType)parser.ReadByte();

            ParseValue();
        }

        protected void ParseValue()
        {
            var type = ((ATAttribute) Command.GetAttr()).ReturnValueType;

            if (parser.HasMoreData()) {
                byte[] buffer = parser.ReadData();

                switch (type) {
                    case ATValueType.None:
                        break;
                    case ATValueType.Number:
                        Value = new ATLongValue().FromByteArray(buffer);
                        break;
                    case ATValueType.HexString:
                        Value = new ATStringValue(ByteUtils.ToBase16(buffer));
                        break;
                    case ATValueType.String:
                        Value = new ATStringValue(Encoding.UTF8.GetString(buffer));
                        break;
                    case ATValueType.NodeIdentifier:
                        Value = new ATNodeIdentifierValue().FromByteArray(buffer);
                        break;
                    case ATValueType.PanDescriptor:
                        Value = new ATPanDescriptorValue().FromByteArray(buffer);
                        break;
                    case ATValueType.NodeDiscover:
                        Value = new ATNodeDiscoverValue().FromByteArray(buffer);
                        break;
                    case ATValueType.NodeDiscoverZB:
                        Value = new ATNodeDiscoverZBValue().FromByteArray(buffer);
                        break;
                }
            }
        }
    }
}
