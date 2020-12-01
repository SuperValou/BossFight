using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Players.LockOns
{
    public class LockOnManager : MonoBehaviour
    {
        // -- Editor

        public float maxLockRange;

        [Header("References")]
        public Camera eye;

        // -- Class

        private readonly HashSet<LockOnTarget> _lockableTargets = new HashSet<LockOnTarget>();

        private LockOnTarget _target;

        public bool IsLocked => _target != null;
        
        public bool TryLockOnTarget()
        {
            Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);

            float minSquaredDistance = float.MaxValue;
            
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(eye);
            
            foreach (var lockableTarget in _lockableTargets)
            {
                Bounds targetBounds = lockableTarget.GetBounds();
                if (!GeometryUtility.TestPlanesAABB(planes, targetBounds))
                {
                    // target is outside of eye frustrum
                    continue;
                }

                // lock target closest to viewport center
                Vector3 viewportPosition = eye.WorldToViewportPoint(lockableTarget.transform.position);
                Vector2 positionFromViewPortCenter = new Vector2(viewportPosition.x - viewportCenter.x, viewportPosition.y - viewportCenter.y);
                float squaredDistance = Vector2.SqrMagnitude(positionFromViewPortCenter);
                if (squaredDistance < minSquaredDistance)
                {
                    minSquaredDistance = squaredDistance;
                    _target = lockableTarget;
                }
            }
            
            return _target != null;
        }

        public LockOnTarget GetTarget()
        {
            if (_target == null)
            {
                throw new InvalidOperationException("Unable to get lock-on target because no target is locked.");
            }

            return _target;
        }

        public void Unlock()
        {
            _target = null;
        }

        public void Register(LockOnTarget lockOnTarget)
        {
            if (lockOnTarget == null)
            {
                throw new ArgumentNullException(nameof(lockOnTarget));
            }

            bool added = _lockableTargets.Add(lockOnTarget);
            if (!added)
            {
                Debug.LogWarning($"{lockOnTarget} ({nameof(LockOnTarget)}) is already registered in {nameof(LockOnManager)}.");
            }
        }

        public void Unregister(LockOnTarget lockOnTarget)
        {
            if (lockOnTarget == null)
            {
                throw new ArgumentNullException(nameof(lockOnTarget));
            }

            bool removed = _lockableTargets.Remove(lockOnTarget);
            if (!removed)
            {
                Debug.LogWarning($"{lockOnTarget} ({nameof(LockOnTarget)}) was not registered in {nameof(LockOnManager)} in the first place.");
            }
        }
    }
}