using System;

namespace Fleck
{
    public class HandshakeException : Exception
    {
        public HandshakeException() { }
        
        public HandshakeException(string message) : base(message) {}
        
        public HandshakeException(string message, Exception innerException) : base(message, innerException) {}
    }
}

