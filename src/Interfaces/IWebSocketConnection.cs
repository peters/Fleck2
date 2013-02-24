using System;

namespace Fleck2.Interfaces
{
    public interface IWebSocketConnection
    {
        Fleck2Extensions.Action OnOpen { get; set; }
        Fleck2Extensions.Action OnClose { get; set; }
        Action<string> OnMessage { get; set; }
        Action<byte[]> OnBinary { get; set; }
        Action<Exception> OnError { get; set; }
        void Send(string message);
        void Send(byte[] message);
        void Close();
        IWebSocketConnectionInfo ConnectionInfo { get; }
    }
}
