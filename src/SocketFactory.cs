using System;
using System.Diagnostics;
using System.Threading;
using Fleck.Interfaces;

namespace Fleck
{
    public class SocketFactory
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
        public SocketFactory(SocketCancellationToken token)
        {
            Token = token;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="beginMethod"></param>
        /// <param name="endMethod"></param>
        /// <param name="resultCallback"> </param>
        /// <returns></returns>
        public void HandleAsync<TResult>(FleckExtensions.Func<AsyncCallback, object, IAsyncResult> beginMethod, 
            FleckExtensions.Func<IAsyncResult, TResult> endMethod, Action<SocketResult> resultCallback)
        {
            DoAsyncTask(() => beginMethod(result => DoAsyncTask(() => 
                resultCallback(new SocketResult(endMethod(result))), resultCallback), null), resultCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginMethod"></param>
        /// <param name="endMethod"></param>
        /// <param name="resultCallback"> </param>
        /// <returns></returns>
        public void HandleAsync(FleckExtensions.Func<AsyncCallback, object, IAsyncResult> beginMethod,
            FleckExtensions.Func<IAsyncResult, ICancellationToken, ISocket> endMethod, Action<SocketResult> resultCallback)
        {
            DoAsyncTask(() => beginMethod(result => DoAsyncTask(() => 
                resultCallback(new SocketResult(endMethod(result, Token))), resultCallback), null), resultCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="beginMethod"></param>
        /// <param name="endMethod"></param>
        /// <param name="resultCallback"> </param>
        /// <returns></returns>
        public void HandleAsyncVoid<T>(FleckExtensions.Func<AsyncCallback, object, T> beginMethod,
            FleckExtensions.Action<T> endMethod, Action<SocketResult> resultCallback)
        {
            DoAsyncTask(() => beginMethod(result => DoAsyncTask(() =>
                {
                    endMethod((T) result);
                    resultCallback(new SocketResult(true));
                }, resultCallback), null), resultCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="resultCallback"> </param>
        private static void DoAsyncTask(FleckExtensions.Action unitOfWork, Action<SocketResult> resultCallback)
        {
            ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        unitOfWork();
                    }
                    catch (SocketCancellationTokenException)
                    {
  
                    } catch(Exception ex)
                    {
                        resultCallback(new SocketResult(ex));
                    }
                });
        }
        #endregion

    }
}