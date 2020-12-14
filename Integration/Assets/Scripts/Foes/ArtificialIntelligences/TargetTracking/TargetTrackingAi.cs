using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking
{
    public abstract class TargetTrackingAi : MonoBehaviour, ITargetTrackingStateMachine
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

        private const string QuietTrigger = "QuietTrigger";
        private const string AlertTrigger = "AlertTrigger";
        private const string HostileTrigger = "HostileTrigger";

        private Animator _animator;
        
        private float _lastLineOfSightTime = 0;

        protected Transform Target { get; set; }

        protected virtual void Start()
        {
            _animator = this.GetOrThrow<Animator>();
            var behaviours = _animator.GetBehaviours<TargetTrackingBehaviour>();
            foreach (var behaviour in behaviours)
            {
                behaviour.Initialize(stateMachine: this);
            }
        }

        public virtual void QuietUpdate()
        {
            if (Target == null)
            {
                return;
            }

            Vector3 targetRelativePosition = Target.position - eye.position;

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
                _animator.SetTrigger(HostileTrigger);
            }
            else
            {
                _animator.SetTrigger(AlertTrigger);
                _lastLineOfSightTime = Time.time;
            }
        }

        public virtual void AlertUpdate()
        {
            if (Target == null)
            {
                _animator.SetTrigger(QuietTrigger);
                return;
            }

            Vector3 targetRelativePosition = Target.position - eye.position;

            if (TargetIsInLineOfSight())
            {
                _lastLineOfSightTime = Time.time;

                if (targetRelativePosition.sqrMagnitude < maxHostileRange * maxHostileRange)
                {
                    // target is now close enough to be attacked
                    _animator.SetTrigger(HostileTrigger);
                }

                return;
            }

            float timeSinceLastLineOfSight = Time.time - _lastLineOfSightTime;
            if (timeSinceLastLineOfSight > brokenLineOfSightTimeout &&
                targetRelativePosition.sqrMagnitude > maxHostileRange * maxHostileRange)
            {
                // target was seen a long time ago and is too far away
                _animator.SetTrigger(QuietTrigger);
            }
        }

        public virtual void HostileUpdate()
        {
            if (Target == null)
            {
                // target destroyed
                _animator.SetTrigger(QuietTrigger);
                return;
            }

            if (TargetIsInLineOfSight())
            {
                Vector3 targetRelativePosition = Target.position - eye.position;

                if (targetRelativePosition.sqrMagnitude < maxHostileRange * maxHostileRange)
                {
                    // target is in direct line of sight and still close enough
                    return;
                }
            }

            // lost line of sight, or target got too far away
            _animator.SetTrigger(AlertTrigger);
            _lastLineOfSightTime = Time.time;
        }

        public virtual void OnBecomeQuiet()
        {
            // do nothing
        }

        public virtual void OnBecomeAlert()
        {
            // do nothing
        }

        public virtual void OnBecomeHostile()
        {
            // do nothing
        }

        public virtual void OnGettingAttacked()
        {
            _animator.SetTrigger(AlertTrigger);
        }

        protected bool TargetIsInLineOfSight()
        {
            if (Target == null)
            {
                return false;
            }

            bool lineOfSightIsObstructed = Physics.Linecast(eye.position, Target.position, out _, blockingLineOfSightLayers, QueryTriggerInteraction.Ignore);
            return !lineOfSightIsObstructed;
        }
    }
}