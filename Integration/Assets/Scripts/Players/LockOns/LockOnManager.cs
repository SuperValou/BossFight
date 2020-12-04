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

        [Header("References")]
        public Camera eye;

        [Tooltip(nameof(ILockOnNotifiable) + " that should be notified when lock/unlock events are occurring.")]
        public MonoBehaviour[] onLockOnEvents;

        // -- Class

        private readonly Vector2 _viewportCenter = new Vector3(0.5f, 0.5f);

        private readonly HashSet<LockOnTarget> _lockableTargets = new HashSet<LockOnTarget>();

        private readonly ICollection<ILockOnNotifiable> _lockOnNotifiables = new HashSet<ILockOnNotifiable>();

        public bool HasTargetLocked { get; private set; }

        public bool HasAnyTargetInSight => Target != null;

        public LockOnTarget Target { get; private set; }

        public Vector2 TargetViewportPosition { get; private set; }
        

        /// <summary>
        /// Position of the nearest target in viewport space (Vector3.zero if no target in sight).
        /// </summary>
        //public Vector3 NearestTargetViewportPosition { get; private set; }
        /// <summary>
        /// Position of the currently locked target in viewport space (Vector3.zero if no target locked).
        /// </summary>
        //public Vector3 LockedTargetViewportPosition { get; private set; }
        void Start()
        {
            foreach (var monoBehaviour in onLockOnEvents)
            {
                _lockOnNotifiables.Add((ILockOnNotifiable) monoBehaviour);
            }
        }

        void Update()
        {
            if (HasTargetLocked)
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
            LockOnTarget previousTarget = Target;
            Target = null;
            TargetViewportPosition = Vector2.zero;

            float minSquaredDistanceToCenter = float.MaxValue;

            foreach (var lockableTarget in _lockableTargets)
            {
                Vector3 targetViewportPosition = eye.WorldToViewportPoint(lockableTarget.transform.position);
                
                var targetPosition = lockableTarget.transform.position - this.transform.position;
                float targetSquaredDistance = Vector3.SqrMagnitude(targetPosition);
                if (IsOutOfRange(targetViewportPosition, targetSquaredDistance))
                {
                    continue;
                }

                Vector2 positionFromViewPortCenter = ((Vector2) targetViewportPosition) - _viewportCenter;
                float squaredDistanceToCenter = Vector2.SqrMagnitude(positionFromViewPortCenter);
                if (squaredDistanceToCenter < minSquaredDistanceToCenter)
                {
                    minSquaredDistanceToCenter = squaredDistanceToCenter;
                    Target = lockableTarget;
                    TargetViewportPosition = targetViewportPosition;
                }
            }

            // Notifications
            if (Target == previousTarget)
            {
                // no change
                return;
            }

            if (Target != null && previousTarget != null)
            {
                // the nearest target is now another target
                return;
            }

            if (previousTarget == null)
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

            if (Target == null)
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
            if (Target == null)
            {
                Debug.LogError($"Attempted to run {nameof(LockedTargetUpdate)}, but {nameof(Target)} was null.");
                return;
            }

            // check if target gets out of range
            var targetPosition = Target.transform.position - this.transform.position;
            float targetSquaredDistance = Vector3.SqrMagnitude(targetPosition);
            Vector2 targetViewportPosition = eye.WorldToViewportPoint(Target.transform.position);
            if (IsOutOfRange(targetViewportPosition, targetSquaredDistance))
            {
                BreakLock();
            }
            else
            {
                TargetViewportPosition = targetViewportPosition;
            }
        }

        public bool TryLockOnTarget()
        {
            if (Target == null)
            {
                // no target in sight
                return false;
            }

            HasTargetLocked = true;

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
            if (Target == null)
            {
                throw new InvalidOperationException("Unable to get lock-on target because no target is locked.");
            }

            return Target;
        }

        public void Unlock()
        {
            HasTargetLocked = false;
            
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
            HasTargetLocked = false;
            
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
    }
}