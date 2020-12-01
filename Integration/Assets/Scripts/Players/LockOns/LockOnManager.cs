using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Players.LockOns
{
    public class LockOnManager : MonoBehaviour
    {
        // -- Editor
 
        [Header("Values")]
        [Tooltip("Viewport dead zone starting from the bottom (percentage between 0 and 0.5).")]
        [Range(0f, 0.5f)]
        public float bottomMargin = 0.2f;

        [Tooltip("Viewport dead zone going to the top (percentage between 0.5 and 1).")]
        [Range(0.5f, 1f)]
        public float topMargin = 0.8f;

        [Tooltip("Maximum distance of the target before breaking lock-on (meters).")]
        public float maxLockRange = 20;

        [Header("References")]
        public Camera eye;

        [Tooltip(nameof(ILockOnNotifiable) + " that should be notified when lock/unlock events are occurring.")]
        public MonoBehaviour[] onLockOnEvents;

        // -- Class

        private readonly HashSet<LockOnTarget> _lockableTargets = new HashSet<LockOnTarget>();
        
        private ICollection<ILockOnNotifiable> _lockOnNotifiables = new HashSet<ILockOnNotifiable>();

        private LockOnTarget _target;
        
        public bool IsLocked => _target != null;

        /// <summary>
        /// Distance to the target (negative value if there is no target)
        /// </summary>
        public float TargetDistance { get; private set; }

        void Start()
        {
            foreach (var monoBehaviour in onLockOnEvents)
            {
                _lockOnNotifiables.Add((ILockOnNotifiable) monoBehaviour);
            }
        }

        void Update()
        {
            if (_target == null)
            {
                return;
            }

            Vector3 targetViewportPosition = eye.WorldToViewportPoint(_target.transform.position);
            TargetDistance = targetViewportPosition.z;

            if (IsOutOfRange(targetViewportPosition))
            {
                BreakLock();
            }
        }

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

                Vector3 viewportPosition = eye.WorldToViewportPoint(lockableTarget.transform.position);

                if (IsOutOfRange(viewportPosition))
                {
                    continue;
                }

                // lock target closest to viewport center
                Vector2 positionFromViewPortCenter = new Vector2(viewportPosition.x - viewportCenter.x, viewportPosition.y - viewportCenter.y);
                float squaredDistance = Vector2.SqrMagnitude(positionFromViewPortCenter);
                if (squaredDistance < minSquaredDistance)
                {
                    minSquaredDistance = squaredDistance;
                    _target = lockableTarget;
                }
            }
            
            if (_target == null)
            {
                return false;
            }

            foreach (var lockOnNotifiable in _lockOnNotifiables)
            {
                try
                {
                    lockOnNotifiable.OnLockOn();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return true;
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
            Unlock(isIntended: true);
        }

        public void BreakLock()
        {
            Unlock(isIntended: false);
        }

        private void Unlock(bool isIntended)
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

        /// <summary>
        /// Returns true if the target is either too far away, or too far in the eye periphery
        /// </summary>
        private bool IsOutOfRange(Vector3 viewportPosition)
        {
            if (viewportPosition.y > topMargin)
            {
                return true;
            }

            if (viewportPosition.y < bottomMargin)
            {
                return true;
            }

            if (viewportPosition.z > maxLockRange)
            {
                return true;
            }

            return false;
        }
    }
}