using UnityEngine;

namespace Assets.Scripts.Players.LockOns
{
    public class LockOnTarget : MonoBehaviour
    {
        // -- Editor

        public LockOnManagerProxy lockOnManagerProxy;

        // -- Class

        void Start()
        {
            lockOnManagerProxy.Register(this);
        }

        void OnDestroy()
        {
            lockOnManagerProxy.Unregister(this);
        }
    }
}