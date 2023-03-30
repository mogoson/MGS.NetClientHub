/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AsyncWorkHandler.cs
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
    public class AsyncWorkHandler : IAsyncWorkHandler
    {
        /// <summary>
        /// Work of handler.
        /// </summary>
        public IAsyncWork Work { protected set; get; }

        /// <summary>
        /// On speed changed event.
        /// </summary>
        public event Action<double> OnSpeedChanged;

        /// <summary>
        /// On progress changed event.
        /// </summary>
        public event Action<float> OnProgressChanged;

        /// <summary>
        /// On completed event.
        /// </summary>
        public event Action<object, Exception> OnCompleted;

        /// <summary>
        /// Last speed value.
        /// </summary>
        protected double speed;

        /// <summary>
        /// Last progress value.
        /// </summary>
        protected float progress;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="work"></param>
        public AsyncWorkHandler(IAsyncWork work)
        {
            Work = work;
        }

        /// <summary>
        /// Notify status of work.
        /// </summary>
        public virtual void NotifyStatus()
        {
            if (speed != Work.Speed)
            {
                speed = Work.Speed;
                InvokeOnSpeedChanged(speed);
            }

            if (progress != Work.Progress)
            {
                progress = Work.Progress;
                InvokeOnProgressChanged(progress);
            }

            if (Work.IsDone)
            {
                if (Work.Result != null || Work.Error != null)
                {
                    InvokeOnCompleted(Work.Result, Work.Error);
                }
            }
        }

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        public virtual void Dispose()
        {
            Work.Dispose();
            Work = null;

            OnSpeedChanged = null;
            OnProgressChanged = null;
            OnCompleted = null;
        }

        /// <summary>
        /// Invoke OnSpeedChanged.
        /// </summary>
        /// <param name="speed"></param>
        protected virtual void InvokeOnSpeedChanged(double speed)
        {
            OnSpeedChanged?.Invoke(speed);
        }

        /// <summary>
        /// Invoke OnProgressChanged.
        /// </summary>
        /// <param name="progress"></param>
        protected virtual void InvokeOnProgressChanged(float progress)
        {
            OnProgressChanged?.Invoke(progress);
        }

        /// <summary>
        /// Invoke OnCompleted.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="error"></param>
        protected virtual void InvokeOnCompleted(object result, Exception error)
        {
            OnCompleted?.Invoke(result, error);
        }
    }
}