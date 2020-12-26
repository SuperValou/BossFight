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

        [Tooltip("Viewport dead zone going to the top (percentage between 0.5 and 1).")] [Range(0.5f, 1f)]
        public float topMargin = 0.8f;

        [Tooltip("Viewport dead zone starting from the left (percentage between 0 and 0.5).")] [Range(0f, 0.5f)]
        public float leftMargin = 0.1f;

        [Tooltip("Viewport dead zone going to the right (percentage between 0.5 and 1).")] [Range(0.5f, 1f)]
        public float rightMargin = 0.9f;
        
        [Tooltip("Maximum distance of the target before breaking lock-on (meters).")]
        public float maxLockRange = 20;

        [Tooltip("Colliders on these layers will block line of sight.")]
        public LayerMask blockingLineOfSightLayers;

        [Header("References")]
        public Camera eye;

        [Tooltip(nameof(ILockOnNotifiable) + " that should be notified when lock/unlock events are occurring.")]
        public MonoBehaviour[] onLockOnEvents;

        // -- Class

        private readonly Vector2 _viewportCenter = new Vector3(0.5f, 0.5f);

        private readonly HashSet<LockOnTarget> _lockableTargets = new HashSet<LockOnTarget>();

        private readonly ICollection<ILockOnNotifiable> _lockOnNotifiables = new HashSet<ILockOnNotifiable>();

        private LockOnTarget _target;
        private bool _isLocked;

        /// <summary>
        /// True if anything is in sight (locked or not)
        /// </summary>
        public bool HasAnyTargetInSight => _target != null;

        /// <summary>
        /// True if a target is locked.
        /// </summary>
        public bool HasTargetLocked => _isLocked && _target != null;

        /// <summary>
        /// Position of the currently locked target in viewport space (Vector3.zero if no target locked).
        /// </summary>
        public Vector2 TargetViewportPosition { get; private set; }
        
        void Start()
        {
            foreach (var monoBehaviour in onLockOnEvents)
            {
                _lockOnNotifiables.Add((ILockOnNotifiable) monoBehaviour);
            }
        }
        
        void LateUpdate()
        {
            if (_isLocked)
            {
                LockedTargetUpdate();
            }
            else
            {
                NearestTargetUpdate();
            }
        }

        private void NearestTargetUpdate()
        {
            LockOnTarget previousTarget = _target;
            _target = null;
            TargetViewportPosition = Vector2.zero;

            float minSquaredDistanceToCenter = float.MaxValue;

            foreach (var lockableTarget in _lockableTargets)
            {
                Vector3 targetViewportPosition = eye.WorldToViewportPoint(lockableTarget.transform.position);
                
                // Check range
                var targetRelativePosition = lockableTarget.transform.position - eye.transform.position;
                float targetSquaredDistance = Vector3.SqrMagnitude(targetRelativePosition);
                if (IsOutOfRange(targetViewportPosition, targetSquaredDistance))
                {
                    continue;
                }

                // Check visibility
                if (Physics.Linecast(eye.transform.position, lockableTarget.transform.position, blockingLineOfSightLayers, QueryTriggerInteraction.Ignore))
                {
                    // target is obscured by something
                    continue;
                }

                // Check if this is the nearest target
                Vector2 positionFromViewPortCenter = ((Vector2) targetViewportPosition) - _viewportCenter;
                float squaredDistanceToCenter = Vector2.SqrMagnitude(positionFromViewPortCenter);
                if (squaredDistanceToCenter < minSquaredDistanceToCenter)
                {
                    minSquaredDistanceToCenter = squaredDistanceToCenter;
                    _target = lockableTarget;
                    TargetViewportPosition = targetViewportPosition;
                }
            }

            // Notifications
            // ReferenceEquals is used because "_target == null" returns true when its gameObject is getting detroyed
            if (ReferenceEquals(_target, previousTarget))
            {
                // no change
                return;
            }

            if (!ReferenceEquals(_target, null) && !ReferenceEquals(previousTarget, null))
            {
                // the nearest target is now another target
                return;
            }

            if (ReferenceEquals(previousTarget, null))
            {
                // a new target appeared
                foreach (var lockOnNotifiable in _lockOnNotifiables)
                {
                    try
                    {
                        lockOnNotifiable.OnLockableInSight();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                return;
            }

            if (ReferenceEquals(_target, null))
            {
                // target disappeared
                foreach (var lockOnNotifiable in _lockOnNotifiables)
                {
                    try
                    {
                        lockOnNotifiable.OnLockableOutOfSight();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                return;
            }
        }

        private void LockedTargetUpdate()
        {
            if (_target == null)
            {
                // Target got destroyed
                Unlock();
                return;
            }

            // check if target gets out of range
            var targetPosition = _target.transform.position - this.transform.position;
            float targetSquaredDistance = Vector3.SqrMagnitude(targetPosition);
            Vector2 targetViewportPosition = eye.WorldToViewportPoint(_target.transform.position);
            if (IsOutOfRange(targetViewportPosition, targetSquaredDistance))
            {
                BreakLock();
            }
            else
            {
                TargetViewportPosition = targetViewportPosition;
            }
        }

        /// <summary>
        /// Attempt to lock the nearest target. Returns wether or not some target got locked.
        /// </summary>
        public bool TryLockOnTarget()
        {
            if (_target == null)
            {
                // no target in sight
                return false;
            }

            _isLocked = true;

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

        /// <summary>
        /// Get the currently locked-on target. Throws if nothing is locked (use <see cref="HasTargetLocked"/> beforehand).
        /// </summary>
        /// <returns></returns>
        public LockOnTarget GetLockedTarget()
        {
            if (_target == null)
            {
                throw new InvalidOperationException("Unable to get lock-on target because no target is locked.");
            }

            return _target;
        }

        /// <summary>
        /// Release the currently locked target (intended by the player).
        /// </summary>
        public void Unlock()
        {
            _isLocked = false;
            
            foreach (var lockOnNotifiable in _lockOnNotifiables)
            {
                try
                {
                    lockOnNotifiable.OnUnlock();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// Release the currently locked target (not intended by the player).
        /// </summary>
        public void BreakLock()
        {
            _isLocked = false;
            
            foreach (var lockOnNotifiable in _lockOnNotifiables)
            {
                try
                {
                    lockOnNotifiable.OnLockBreak();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        
        /// <summary>
        /// Returns true if the target is either out of sight, too far away or to close to periphery
        /// </summary>
        private bool IsOutOfRange(Vector3 viewportPosition, float squaredDistance)
        {
            if (viewportPosition.z < 0)
            {
                // is behind us
                return true;
            }

            if (viewportPosition.x < leftMargin 
                || viewportPosition.x > rightMargin
                || viewportPosition.y < bottomMargin 
                || viewportPosition.y > topMargin)
            {
                // is out of sight or too close to periphery
                return true;
            }

            if (squaredDistance > maxLockRange * maxLockRange)
            {
                // is too far away
                return true;
            }

            return false;
        }

        /// <summary>
        /// Register a <see cref="LockOnTarget"/> into the system (should only be used during <see cref="LockOnTarget"/> initialization)
        /// </summary>
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

        /// <summary>
        /// Unregister a <see cref="LockOnTarget"/> out of the system (should only be used during <see cref="LockOnTarget"/> end of life)
        /// </summary>
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