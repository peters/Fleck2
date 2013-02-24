using System;
using Fleck2.Interfaces;

namespace Fleck2
{
    public class SocketCancellationToken : ICancellationToken
    {

        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly object _syncLock = new object();
        #endregion

        #region Fields
        /// <summary>
        /// 
        /// </summary>
        public readonly Guid Token = new Guid();

        /// <summary>
        /// 
        /// </summary>
        private bool _isCancellationRequested;
        /// <summary>
        /// 
        /// </summary>
        public bool IsCancellationRequested
        {
            get
            {
                lock(_syncLock)
                {
                    return _isCancellationRequested;
                }
            }
            private set
            {
                lock(_syncLock)
                {
                    _isCancellationRequested = value;
                }
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// 
        /// </summary>
        public void ThrowIfCancellationRequested()
        {
            if (IsCancellationRequested)
            {
                throw new SocketCancellationTokenException(this);
            }                
        }
        /// <summary>
        /// 
        /// </summary>
        public void Cancel()
        {
            IsCancellationRequested = true;                
        }
        #endregion

    }
}