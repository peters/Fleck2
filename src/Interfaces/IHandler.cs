using System.Collections.Generic;

namespace Fleck.Interfaces
{
    public interface IHandler
    {
        byte[] CreateHandshake();
        void Receive(IEnumerable<byte> data);
        byte[] FrameText(string text);
        byte[] FrameBinary(byte[] bytes);
        byte[] FrameClose(int code);
    }
}

