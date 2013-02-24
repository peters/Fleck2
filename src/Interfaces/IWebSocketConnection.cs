using System;

namespace Fleck.Interfaces
{
    public interface IWebSocketConnection
    {
        FleckExtensions.Action OnOpen { get; set; }
        FleckExtensions.Action OnClose { get; set; }
        Action<string> OnMessage { get; set; }
        Action<byte[]> OnBinary { get; set; }
        Action<Exception> OnError { get; set; }
        void Send(string message);
        void Send(byte[] message);
        void Close();
        IWebSocketConnectionInfo ConnectionInfo { get; }
    }
}
