/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetMonoHub.cs
 *  Description  :  Hub to manage net clients and cache net
 *                  data, and unity main thread notify status.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  03/10/2023
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections;
using UnityEngine;

namespace MGS.Net
{
    /// <summary>
    /// Hub to manage net clients and cache net data,
    /// and main thread notify status.
    /// </summary>
    public class NetMonoHub : NetBridgeHub, INetMonoHub
    {
        /// <summary>
        /// Behaviour for hub to StartCoroutine.
        /// </summary>
        protected MonoBehaviour behaviour;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resultCacher">Cacher for net result.</param>
        /// <param name="clientCacher">Cacher for net client.</param>
        /// <param name="concurrency">Max count of concurrency clients.</param>
        /// <param name="resolver">Net resolver to check retrieable.</param>
        public NetMonoHub(ICacher<string> resultCacher = null,
            ICacher<INetClient> clientCacher = null, int concurrency = 3, INetResolver resolver = null)
            : base(resultCacher, clientCacher, concurrency, resolver)
        {
            behaviour = new GameObject("NetBehaviour").AddComponent<MonoBehaviour>();
            behaviour.StartCoroutine(BehaviourTick());
            Object.DontDestroyOnLoad(behaviour.gameObject);
        }

        /// <summary>
        /// Dispose all resource.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Object.Destroy(behaviour.gameObject);
            behaviour = null;
        }

        /// <summary>
        /// Behaviour tick to update.
        /// </summary>
        /// <returns></returns>
        private IEnumerator BehaviourTick()
        {
            while (!isDisposed)
            {
                Update();
                yield return null;
            }
        }
    }
}