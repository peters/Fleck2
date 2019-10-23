using System;
using System.Collections.Generic;
using Fleck2.Interfaces;

namespace Fleck2.Handlers
{
    public class ComposableHandler : IHandler
    {
        public Fleck2Extensions.Func<byte[]> Handshake = () => new byte[0];
        public Fleck2Extensions.Func<string, byte[]> TextFrame = x => new byte[0];
        public Fleck2Extensions.Func<byte[], byte[]> BinaryFrame = x => new byte[0];
        public Action<List<byte>> ReceiveData = delegate { };
        public Fleck2Extensions.Func<int, byte[]> CloseFrame = i => new byte[0];
        
        private readonly List<byte> _data = new List<byte>();

        public byte[] CreateHandshake()
        {
            return Handshake();
        }

        public void Receive(IEnumerable<byte> data)
        {
            _data.AddRange(data);
            
            ReceiveData(_data);
        }
        
        public byte[] FrameText(string text)
        {
            return TextFrame(text);
        }
        
        public byte[] FrameBinary(byte[] bytes)
        {
            return BinaryFrame(bytes);
        }
        
        public byte[] FrameClose(int code)
        {
            return CloseFrame(code);
        }
    }
}

