using System;
using System.Threading;
using Fleck2.Interfaces;

namespace Fleck2
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
        public void HandleAsync<TResult>(Fleck2Extensions.Func<AsyncCallback, object, IAsyncResult> beginMethod, 
            Fleck2Extensions.Func<IAsyncResult, TResult> endMethod, Action<SocketResult> resultCallback)
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
        public void HandleAsync(Fleck2Extensions.Func<AsyncCallback, object, IAsyncResult> beginMethod,
            Fleck2Extensions.Func<IAsyncResult, ICancellationToken, ISocket> endMethod, Action<SocketResult> resultCallback)
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
        public void HandleAsyncVoid<T>(Fleck2Extensions.Func<AsyncCallback, object, T> beginMethod,
            Fleck2Extensions.Action<T> endMethod, Action<SocketResult> resultCallback)
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
        private static void DoAsyncTask(Fleck2Extensions.Action unitOfWork, Action<SocketResult> resultCallback)
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