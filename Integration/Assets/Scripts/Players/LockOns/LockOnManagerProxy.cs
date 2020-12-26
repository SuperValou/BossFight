using UnityEngine;

namespace Assets.Scripts.Players.LockOns
{
    public class LockOnManagerProxy : MonoBehaviour
    {
        private LockOnManager _lockOnManager;

        void Awake()
        {
            _lockOnManager = GameObject.FindObjectOfType<LockOnManager>();
            if (_lockOnManager == null)
            {
                Debug.LogError($"Unable to find {nameof(LockOnManager)} in hierarchy. Lock-on feature will not work.");
            }
        }

        public void Register(LockOnTarget lockOnTarget)
        {
            _lockOnManager?.Register(lockOnTarget);
        }

        public void Unregister(LockOnTarget lockOnTarget)
        {
            _lockOnManager?.Unregister(lockOnTarget);
        }
    }
}