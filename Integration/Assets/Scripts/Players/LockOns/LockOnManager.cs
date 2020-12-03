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

        private readonly Vector3 _viewportCenter = new Vector3(0.5f, 0.5f, 0);

        private readonly HashSet<LockOnTarget> _lockableTargets = new HashSet<LockOnTarget>();

        private readonly ICollection<ILockOnNotifiable> _lockOnNotifiables = new HashSet<ILockOnNotifiable>();

        private LockOnTarget _nearestTarget;
        private LockOnTarget _lockedTarget;

        public bool HasAnyTargetInSight => _nearestTarget != null;
        public bool HasTargetLocked => _lockedTarget != null;


        /// <summary>
        /// Position of the nearest target in viewport space (Vector3.zero if no target in sight).
        /// </summary>
        public Vector3 NearestTargetViewportPosition { get; private set; }

        /// <summary>
        /// Position of the currently locked target in viewport space (Vector3.zero if no target locked).
        /// </summary>
        public Vector3 LockedTargetViewportPosition { get; private set; }

        void Start()
        {
            foreach (var monoBehaviour in onLockOnEvents)
            {
                _lockOnNotifiables.Add((ILockOnNotifiable)monoBehaviour);
            }
        }

        void Update()
        {
            ClosestTargetUpdate();
            CurrentTargetUpdate();
        }

        private void ClosestTargetUpdate()
        {
            LockOnTarget previousNearestTarget = _nearestTarget;
            _nearestTarget = null;
            NearestTargetViewportPosition = Vector3.zero;

            float minSquaredDistanceToCenter = float.MaxValue;

            foreach (var lockableTarget in _lockableTargets)
            {
                if (lockableTarget == _lockedTarget)
                {
                    continue;
                }

                Vector3 lockableTargetViewportPosition = eye.WorldToViewportPoint(lockableTarget.transform.position);
                if (lockableTargetViewportPosition == Vector3.zero)
                {
                    // target is outside of viewport
                    continue;
                }

                if (IsOutOfRange(lockableTargetViewportPosition))
                {
                    continue;
                }

                Vector2 positionFromViewPortCenter = new Vector2(lockableTargetViewportPosition.x - _viewportCenter.x, lockableTargetViewportPosition.y - _viewportCenter.y);
                float squaredDistance = Vector2.SqrMagnitude(positionFromViewPortCenter);
                if (squaredDistance < minSquaredDistanceToCenter)
                {
                    minSquaredDistanceToCenter = squaredDistance;
                    _nearestTarget = lockableTarget;
                    NearestTargetViewportPosition = lockableTargetViewportPosition;
                }
            }

            // Notifications
            if (_nearestTarget == previousNearestTarget)
            {
                // no change
                return;
            }

            if (_nearestTarget != null && previousNearestTarget != null)
            {
                // the nearest target is now another target
                return;
            }

            if (previousNearestTarget == null)
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

            if (_nearestTarget == null)
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

        private void CurrentTargetUpdate()
        {
            if (_lockedTarget == null)
            {
                return;
            }

            LockedTargetViewportPosition = eye.WorldToViewportPoint(_lockedTarget.transform.position);

            if (IsOutOfRange(LockedTargetViewportPosition))
            {
                BreakLock();
            }
        }

        public bool TryLockOnTarget()
        {
            if (_nearestTarget == null)
            {
                // no target in sight
                return false;
            }

            _lockedTarget = _nearestTarget;
            LockedTargetViewportPosition = NearestTargetViewportPosition;

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

        public LockOnTarget GetLockedTarget()
        {
            if (_lockedTarget == null)
            {
                throw new InvalidOperationException("Unable to get lock-on target because no target is locked.");
            }

            return _lockedTarget;
        }

        public void Unlock()
        {
            _lockedTarget = null;
            LockedTargetViewportPosition = Vector3.zero;
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

        public void BreakLock()
        {
            _lockedTarget = null;
            LockedTargetViewportPosition = Vector3.zero;
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