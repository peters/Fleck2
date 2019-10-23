using System;

namespace Fleck2
{
    public class SocketCancellationTokenException : Exception
    {

        #region Fields
        /// <summary>
        /// 
        /// </summary>
        public SocketCancellationToken Token { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public SocketCancellationTokenException(SocketCancellationToken token)
        {
            Token = token;
        }
        #endregion

    }
}