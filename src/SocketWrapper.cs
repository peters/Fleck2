using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Fleck2.Interfaces;

namespace Fleck2
{
    public class SocketWrapper : ISocket
    {
        private readonly Socket _socket;
        private Stream _stream;
        private readonly SocketCancellationToken _socketCancellationToken;
        private readonly SocketFactory _socketFactory;
        
        public string RemoteIpAddress
        {
            get
            {
                var endpoint = _socket.RemoteEndPoint as IPEndPoint;
                return endpoint != null ? endpoint.Address.ToString() : null;
            }
        }

        public int RemotePort
        {
            get
            {
                var endpoint = _socket.RemoteEndPoint as IPEndPoint;
                return endpoint != null ? endpoint.Port : -1;
            }
        }

        public SocketWrapper(Socket socket)
        {
            _socketCancellationToken = new SocketCancellationToken();
            _socketFactory = new SocketFactory(_socketCancellationToken);
            _socket = socket;
            if (_socket.Connected)
                _stream = new NetworkStream(_socket);
        }

        public void Authenticate(X509Certificate2 certificate, Fleck2Extensions.Action callback, Fleck2Extensions.Action<Exception> error)
        {
            var ssl = new SslStream(_stream, false);
            _stream = ssl;

            Fleck2Extensions.Func<AsyncCallback, object, IAsyncResult> begin =
                (cb, s) => ssl.BeginAuthenticateAsServer(certificate, false, SslProtocols.Tls, false, cb, s);

            _socketFactory.HandleAsyncVoid(begin, ssl.EndAuthenticateAsServer, result =>
                {
                    result.Success(callback);
                    result.Error(error);
                });

        }

        public void Listen(int backlog)
        {
            _socket.Listen(backlog);
        }

        public void Bind(EndPoint endPoint)
        {
            _socket.Bind(endPoint);
        }

        public bool Connected
        {
            get { return _socket.Connected; }
        }
        
        public Stream Stream
        {
            get { return _stream; }
        }

        public bool NoDelay
        {
            get { return _socket.NoDelay; }
            set { _socket.NoDelay = value; }
        }

        public void Receive(byte[] buffer, Fleck2Extensions.Action<int> callback, Fleck2Extensions.Action<Exception> error, int offset = 0)
        {
      
            Fleck2Extensions.Func<AsyncCallback, object, IAsyncResult> begin = (cb, data) =>
            _stream.BeginRead(buffer, offset, buffer.Length, cb, data);

            _socketFactory.HandleAsync(begin, _stream.EndRead, result =>
            {
                result.Success(callback);
                result.Error(error);
            });

        }

        public void Accept(Fleck2Extensions.Action<ISocket> callback, Fleck2Extensions.Action<Exception> error)
        {

            Fleck2Extensions.Func<IAsyncResult, ICancellationToken, ISocket> end = (result, token) =>
            {
                token.ThrowIfCancellationRequested();
                return new SocketWrapper(_socket.EndAccept(result));
            };

            _socketFactory.HandleAsync(_socket.BeginAccept, end, result =>
            {
                result.Success(callback);
                result.Error(error);
            });

        }

        public void Dispose()
        {
            _socketCancellationToken.Cancel();
            if (_stream != null) _stream.Dispose();
        }

        public void Close()
        {
            _socketCancellationToken.Cancel();
            if (_stream != null) _stream.Close();
            if (_socket != null) _socket.Close();
        }

        public int EndSend(IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
            return 0;
        }

        public void Send(byte[] buffer, Fleck2Extensions.Action callback, Fleck2Extensions.Action<Exception> error)
        {

            Fleck2Extensions.Func<AsyncCallback, object, IAsyncResult> begin =
                            (cb, s) => _stream.BeginWrite(buffer, 0, buffer.Length, cb, s);

            _socketFactory.HandleAsyncVoid(begin, _stream.EndWrite, result =>
            {
                result.Success(callback);
                result.Error(error);
            });      

        }
    }


}
