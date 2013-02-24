using System;

namespace Fleck2
{
    public class WebSocketException : Exception
    {
        public WebSocketException(ushort statusCode)
        {
            StatusCode = statusCode;
        }
        
        public WebSocketException(ushort statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
        
        public WebSocketException(ushort statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
        
        public ushort StatusCode { get; private set;}
    }
}
