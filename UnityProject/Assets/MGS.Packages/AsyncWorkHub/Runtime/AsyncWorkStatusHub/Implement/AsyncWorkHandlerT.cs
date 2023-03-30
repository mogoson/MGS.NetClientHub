/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AsyncWorkHandlerT.cs
 *  Description  :  Handler to manage work status.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  03/10/2023
 *  Description  :  Initial development version.
 *************************************************************************/

using System;

namespace MGS.Work
{
    /// <summary>
    /// Handler to manage work status.
    /// </summary>
    public class AsyncWorkHandler<T> : AsyncWorkHandler, IAsyncWorkHandler<T>
    {
        /// <summary>
        /// Work of handler.
        /// </summary>
        public new IAsyncWork<T> Work
        {
            protected set { base.Work = value; }
            get { return base.Work as IAsyncWork<T>; }
        }

        /// <summary>
        /// On completed event.
        /// </summary>
        public new event Action<T, Exception> OnCompleted;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="work"></param>
        public AsyncWorkHandler(IAsyncWork<T> work) : base(work) { }

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            OnCompleted = null;
        }

        /// <summary>
        /// Invoke OnCompleted.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="error"></param>
        protected override void InvokeOnCompleted(object result, Exception error)
        {
            base.InvokeOnCompleted(result, error);
            this.InvokeOnCompleted(Work.Result, error);
        }

        /// <summary>
        /// Invoke OnCompleted.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="error"></param>
        protected virtual void InvokeOnCompleted(T result, Exception error)
        {
            OnCompleted?.Invoke(result, error);
        }
    }
}