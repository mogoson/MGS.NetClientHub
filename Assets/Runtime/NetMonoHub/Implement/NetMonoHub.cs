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
using MGS.Caches;
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
        /// NetBehaviour to handle MonoBehaviour.
        /// </summary>
        protected class NetBehaviour : MonoBehaviour { }

        /// <summary>
        /// Behaviour for hub to StartCoroutine.
        /// </summary>
        protected NetBehaviour behaviour;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="clientCacher">Cacher for net client.</param>
        /// <param name="resultCacher">Cacher for net result.</param>
        /// <param name="concurrency">Max count of concurrency clients.</param>
        /// <param name="resolver">Net resolver to check retrieable.</param>
        public NetMonoHub(ICacher<INetClient> clientCacher = null, ICacher<object> resultCacher = null,
            int concurrency = 3, INetResolver resolver = null)
            : base(clientCacher, resultCacher, concurrency, resolver)
        {
            behaviour = new GameObject(typeof(NetBehaviour).Name).AddComponent<NetBehaviour>();
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