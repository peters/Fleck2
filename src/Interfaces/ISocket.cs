#define NET20

using System;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Fleck2.Interfaces
{
    public interface ISocket
    {
        bool Connected { get; }
        string RemoteIpAddress { get; }
        int RemotePort { get; }
        Stream Stream { get; }
        bool NoDelay { get; set; }

        void Accept(Fleck2Extensions.Action<ISocket> callback, Fleck2Extensions.Action<Exception> error);
        void Send(byte[] buffer, Fleck2Extensions.Action callback, Fleck2Extensions.Action<Exception> error);
        void Receive(byte[] buffer, Fleck2Extensions.Action<int> callback, Fleck2Extensions.Action<Exception> error, int offset);
        void Authenticate(X509Certificate2 certificate, Fleck2Extensions.Action callback, Fleck2Extensions.Action<Exception> error);

        void Dispose();
        void Close();

        void Bind(EndPoint ipLocal);
        void Listen(int backlog);
    }
}
