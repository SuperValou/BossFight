using System;
using Assets.Scripts.Damages;
using UnityEngine;

namespace Assets.Scripts.Foes.ArtificialIntelligences
{
    public class TargetTracker : MonoBehaviour
    {
        // -- Editor

        [Header("Values")] [Tooltip("The max distance at which detection can occur (meters).")]
        public float maxDetectionRange = 50;

        [Tooltip("The max distance at which hostile behaviour will hold (meters).")]
        public float maxHostileRange = 30;

        [Tooltip("Time before letting go after line of sight to target is broken (seconds).")]
        public float brokenLineOfSightTimeout = 5;

        [Tooltip("Colliders on these layers will block line of sight.")]
        public LayerMask blockingLineOfSightLayers;

        [Header("Parts")] [Tooltip("Source point of target detection.")]
        public Transform eye;

        // -- Class

        private Damageable _target;

        private float _lastLineOfSightTime = 0;

        /// <summary>
        /// Current <see cref="TargetTrackingState"/>.
        /// </summary>
        public TargetTrackingState State { get; set; } = TargetTrackingState.Quiet;

        /// <summary>
        /// True on the frame where <see cref="State"/> changed.
        /// </summary>
        public bool OnStateChanged { get; private set; }

        void Update()
        {
            var previousState = State;

            switch (previousState)
            {
                case TargetTrackingState.Quiet:
                    QuietUpdate();
                    break;

                case TargetTrackingState.LookOut:
                    LookOutUpdate();
                    break;

                case TargetTrackingState.Hostile:
                    HostileUpdate();
                    break;

                default:
                    throw new InvalidOperationException($"Unhandled {nameof(TargetTrackingState)}: {State}");
            }
            
            OnStateChanged = previousState != State;
        }

        private void QuietUpdate()
        {
            if (_target == null)
            {
                return;
            }

            Vector3 targetRelativePosition = _target.transform.position - eye.position;

            if (targetRelativePosition.sqrMagnitude > maxDetectionRange * maxDetectionRange)
            {
                // too far away
                return;
            }

            if (!TargetIsInLineOfSight())
            {
                // obstructed line of sight
                return;
            }

            if (targetRelativePosition.sqrMagnitude < maxHostileRange * maxHostileRange)
            {
                State = TargetTrackingState.Hostile;
            }
            else
            {
                State = TargetTrackingState.LookOut;
                _lastLineOfSightTime = Time.time;
            }
        }

        private void LookOutUpdate()
        {
            if (_target == null)
            {
                State = TargetTrackingState.Quiet;
                return;
            }

            Vector3 targetRelativePosition = _target.transform.position - eye.position;

            //if (targetRelativePosition.sqrMagnitude > maxDetectionRange * maxDetectionRange)
            //{
            //    // target is too far away
            //    State = TargetTrackingState.Quiet;
            //    return;
            //}

            if (TargetIsInLineOfSight())
            {
                _lastLineOfSightTime = Time.time;

                if (targetRelativePosition.sqrMagnitude < maxHostileRange * maxHostileRange)
                {
                    State = TargetTrackingState.Hostile;
                }
            }
            else
            {
                if (_lastLineOfSightTime > brokenLineOfSightTimeout &&
                    targetRelativePosition.sqrMagnitude > maxHostileRange * maxHostileRange)
                {
                    State = TargetTrackingState.Quiet;
                }
            }
        }

        private void HostileUpdate()
        {
            if (_target == null)
            {
                // target destroyed
                State = TargetTrackingState.Quiet;
                return;
            }

            if (TargetIsInLineOfSight())
            {
                return;
            }

            State = TargetTrackingState.LookOut;
            _lastLineOfSightTime = Time.time;
        }

        private bool TargetIsInLineOfSight()
        {
            bool lineOfSightIsObstructed = Physics.Linecast(eye.position, _target.transform.position, out _,
                blockingLineOfSightLayers, QueryTriggerInteraction.Ignore);
            return !lineOfSightIsObstructed;
        }

        public void SetTarget(Damageable target)
        {
            _target = target;
        }
    }
}