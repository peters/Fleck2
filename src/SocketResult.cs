using System;

namespace Fleck2
{
    public class SocketResult
    {

        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly object _result;
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public SocketResult(object result)
        {
            _result = result;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        public SocketResult Success<TResult>(Fleck2Extensions.Action<TResult> callback)
        {
            if(!(_result is Exception))
            {
                callback((TResult) _result);
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public SocketResult Error<TResult>(Fleck2Extensions.Action<TResult> callback)
        {
            if (_result is Exception)
            {
                callback((TResult)_result);
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public SocketResult Success(Fleck2Extensions.Action callback)
        {
            if(!(_result is Exception))
            {
                callback();
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult AsValue<TResult>()
        {
            return (TResult) _result;
        }
        #endregion

    }
}