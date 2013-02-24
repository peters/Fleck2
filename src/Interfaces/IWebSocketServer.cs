using System;

namespace Fleck.Interfaces
{
    public interface IWebSocketServer : IDisposable
    {
        void Start(Action<IWebSocketConnection> config);
    }
}
