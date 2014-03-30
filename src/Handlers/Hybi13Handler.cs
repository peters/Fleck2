using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Fleck2.Interfaces;

using Ex = Fleck2.Fleck2Extensions;
using IEx = Fleck2.IntExtensions;

namespace Fleck2.Handlers
{
    public static class Hybi13Handler
    {
        public static IHandler Create(WebSocketHttpRequest request, Action<string> onMessage, Fleck2Extensions.Action onClose, Action<byte[]> onBinary)
        {
            var readState = new ReadState();
            return new ComposableHandler
            {
                Handshake = () => BuildHandshake(request),
                TextFrame = data => FrameData(Encoding.UTF8.GetBytes(data), FrameType.Text),
                BinaryFrame = data => FrameData(data, FrameType.Binary),
                CloseFrame = i => FrameData(IEx.ToBigEndianBytes<ushort>(i), FrameType.Close),
                ReceiveData = bytes => ReceiveData(bytes, readState, (op, data) => ProcessFrame(op, data, onMessage, onClose, onBinary))
            };
        }
        
        public static byte[] FrameData(byte[] payload, FrameType frameType)
        {
            var memoryStream = new MemoryStream();
            var op = (byte)((byte)frameType + 128);
            
            memoryStream.WriteByte(op);
            
            if (payload.Length > UInt16.MaxValue) {
                memoryStream.WriteByte(127);
                var lengthBytes = IEx.ToBigEndianBytes<ulong>(payload.Length);
                memoryStream.Write(lengthBytes, 0, lengthBytes.Length);
            } else if (payload.Length > 125) {
                memoryStream.WriteByte(126);
                var lengthBytes = IEx.ToBigEndianBytes<ushort>(payload.Length);
                memoryStream.Write(lengthBytes, 0, lengthBytes.Length);
            } else {
                memoryStream.WriteByte((byte)payload.Length);
            }
            
            memoryStream.Write(payload, 0, payload.Length);
            
            return memoryStream.ToArray();
        }
        
        public static void ReceiveData(List<byte> data, ReadState readState, Fleck2Extensions.Action<FrameType, byte[]> processFrame)
        {
            while (data.Count >= 2)
            {
                var isFinal = (data[0] & 128) != 0;
                var reservedBits = (data[0] & 112);
                var frameType = (FrameType)(data[0] & 15);
                var isMasked = (data[1] & 128) != 0;
                var length = (data[1] & 127);
                
                
                if (!isMasked
                    || !Enum.IsDefined(typeof(FrameType), frameType)
                    || reservedBits != 0 //Must be zero per spec 5.2
                    || (frameType == FrameType.Continuation && !readState.FrameType.HasValue))
                    throw new WebSocketException(WebSocketStatusCodes.ProtocolError);
                
                var index = 2;
                int payloadLength;
                
                switch (length)
                {
                    case 127:
                        if (data.Count < index + 8)
                            return; //Not complete
                        payloadLength = IEx.ToLittleEndianInt(Ex.ToArray(Ex.Take(Ex.Skip(data, index), (8))));
                        index += 8;
                        break;
                    case 126:
                        if (data.Count < index + 2)
                            return; //Not complete
                        payloadLength = IEx.ToLittleEndianInt(Ex.ToArray(Ex.Take(Ex.Skip(data, index), 2)));
                        index += 2;
                        break;
                    default:
                        payloadLength = length;
                        break;
                }
                
                if (data.Count < index + 4) 
                    return; //Not complete

                var maskBytes = Ex.ToList(Ex.Take(Ex.Skip(data, index), 4)); 
 
                index += 4;

                if (data.Count < index + payloadLength) 
                    return; //Not complete

                var i = 0;
                var payload = Ex.Select(Ex.Take(Ex.Skip(data, index), payloadLength),
                              value => (byte)(value ^ maskBytes[i++ % 4]));
               
                readState.Data.AddRange(payload);
                data.RemoveRange(0, index + payloadLength);
                
                if (frameType != FrameType.Continuation)
                    readState.FrameType = frameType;

                if (!isFinal || !readState.FrameType.HasValue)
                {
                    continue;
                }

                var stateData = readState.Data.ToArray();
                var stateFrameType = readState.FrameType;
                readState.Clear();
                    
                processFrame(stateFrameType.Value, stateData);
            }
        }
        
        public static void ProcessFrame(FrameType frameType, byte[] data, Action<string> onMessage, Fleck2Extensions.Action onClose, Action<byte[]> onBinary)
        {
            switch (frameType)
            {
            case FrameType.Close:
                if (data.Length == 1 || data.Length>125)
                    throw new WebSocketException(WebSocketStatusCodes.ProtocolError);
                    
                if (data.Length >= 2)
                {
                    var closeCode = (ushort) (IEx.ToLittleEndianInt(Ex.ToArray(Ex.Take(data, 2))));
                    if (!WebSocketStatusCodes.Contains(closeCode) && (closeCode < 3000 || closeCode > 4999))
                        throw new WebSocketException(WebSocketStatusCodes.ProtocolError);
                }
                
                if (data.Length > 2)
                    ReadUtf8PayloadData(Ex.Skip(data, 2));
                
                onClose();
                break;
            case FrameType.Binary:
                onBinary(data);
                break;
            case FrameType.Text:
                onMessage(ReadUtf8PayloadData(data));
                break;
            default:
                FleckLog.Debug("Received unhandled " + frameType,null);
                break;
            }
        }
        
        
        public static byte[] BuildHandshake(WebSocketHttpRequest request)
        {
            FleckLog.Debug("Building Hybi-14 Response",null);
            
            var builder = new StringBuilder();

            builder.Append("HTTP/1.1 101 Switching Protocols\r\n");
            builder.Append("Upgrade: websocket\r\n");
            builder.Append("Connection: Upgrade\r\n");

            var responseKey =  CreateResponseKey(request["Sec-WebSocket-Key"]);
            builder.AppendFormat("Sec-WebSocket-Accept: {0}\r\n", responseKey);
            builder.Append("\r\n");

            return Encoding.ASCII.GetBytes(builder.ToString());
        }

        private const string WebSocketResponseGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        
        public static string CreateResponseKey(string requestKey)
        {
            var combined = requestKey + WebSocketResponseGuid;

            var bytes = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(combined));

            return Convert.ToBase64String(bytes);
        }
        
        private static string ReadUtf8PayloadData(byte[] bytes)
        {
            var encoding = new UTF8Encoding(false, true);
            try
            {
                return encoding.GetString(bytes);
            }
            catch(ArgumentException)
            {
                throw new WebSocketException(WebSocketStatusCodes.InvalidFramePayloadData);
            }
        }
    }

}
