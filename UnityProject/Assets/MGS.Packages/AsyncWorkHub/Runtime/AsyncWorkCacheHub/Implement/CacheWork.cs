/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  CacheWork.cs
 *  Description  :  Work for cache data.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.Work
{
    /// <summary>
    /// Work for cache data.
    /// </summary>
    public class CacheWork : AsyncWork
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="result">Result cache data.</param>
        public CacheWork(object result)
        {
            Result = result;
            IsDone = true;
        }

        /// <summary>
        /// Execute work operation.
        /// </summary>
        public override void Execute() { }
    }
}