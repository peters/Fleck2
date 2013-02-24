#define NET20

using System;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Fleck.Interfaces
{
    public interface ISocket
    {
        bool Connected { get; }
        string RemoteIpAddress { get; }
        int RemotePort { get; }
        Stream Stream { get; }
        bool NoDelay { get; set; }

        void Accept(FleckExtensions.Action<ISocket> callback, FleckExtensions.Action<Exception> error);
        void Send(byte[] buffer, FleckExtensions.Action callback, FleckExtensions.Action<Exception> error);
        void Receive(byte[] buffer, FleckExtensions.Action<int> callback, FleckExtensions.Action<Exception> error, int offset = 0);
        void Authenticate(X509Certificate2 certificate, FleckExtensions.Action callback, FleckExtensions.Action<Exception> error);

        void Dispose();
        void Close();

        void Bind(EndPoint ipLocal);
        void Listen(int backlog);
    }
}
