/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AsyncWorkT.cs
 *  Description  :  Async work abstract implement.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.Work
{
    /// <summary>
    /// Async work abstract implement.
    /// </summary>
    public abstract class AsyncWork<T> : AsyncWork, IAsyncWork<T>
    {
        /// <summary>
        /// Result of work.
        /// </summary>
        public new virtual T Result
        {
            protected set { base.Result = value; }
            get { return (T)base.Result; }
        }
    }
}