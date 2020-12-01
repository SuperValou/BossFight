using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Players.LockOns
{
    public class LockOnTarget : MonoBehaviour
    {
        // -- Editor

        public LockOnManagerProxy lockOnManagerProxy;

        // -- Class

        private Renderer _renderer;

        void Start()
        {
            _renderer = this.GetOrThrow<Renderer>();

            lockOnManagerProxy.Register(this);
        }

        void OnDestroy()
        {
            lockOnManagerProxy.Unregister(this);
        }

        public Bounds GetBounds()
        {
            return _renderer.bounds;
        }
    }
}