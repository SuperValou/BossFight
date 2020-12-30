using Assets.Scripts.Foes.ArtificialIntelligences;
using Assets.Scripts.Players;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes.Shells
{
    public class ShellAi : MonoBehaviour, IStateMachine
    {
        // -- Editor

        [Header("Values")] [Tooltip("Speed of movement while rolling (m/s).")]
        public float rollSpeed = 5;

        [Tooltip("Distance to travel when rolling (meters).")]
        public float rollDistance = 10;

        [Tooltip("Shell's radius when rolling (meters).")]
        public float bodyRadius = 2;

        [Tooltip("Layers of the environment to let Shell detects where to roll.")]
        public LayerMask environmentLayers;

        [Header("Parts")] public Transform body;
        public ProjectileEmitter shockwaveEmitter;
        public ProjectileEmitter laserWallEmitter;

        [Header("References")] public PlayerProxy playerProxy;

        // -- Class

        private const string InitializedBool = "IsInitialized";
        private const string LaserWallAttackTrigger = "LaserWallAttackTrigger";
        private const string ShockwaveTrigger = "ShockwaveTrigger";

        private const string RollBeginTrigger = "RollBeginTrigger";
        private const string RollEndTrigger = "RollEndTrigger";


        private Rigidbody _rigidbody;
        private Animator _animator;

        private Vector3 rollDirection;
        private Vector3 rollDestination;
        private Vector3 rollAxis;

        void Start()
        {
            _rigidbody = this.GetOrThrow<Rigidbody>();
            _animator = this.GetOrThrow<Animator>();
            var behaviours = _animator.GetBehaviours<Behaviour<ShellAi>>();
            foreach (var behaviour in behaviours)
            {
                behaviour.Initialize(stateMachine: this);
            }

            _animator.SetBool(InitializedBool, value: true);
        }

        public void OnIdle()
        {
            int rand = ((int) (Random.value * 10)) % 3;
            if (rand == 0)
            {
                _animator.SetTrigger(LaserWallAttackTrigger);
            }
            else if (rand == 1)
            {
                _animator.SetTrigger(ShockwaveTrigger);
            }
            else if (rand == 2)
            {
                _animator.SetTrigger(RollBeginTrigger);
            }
        }

        public void DoLaserWallAttack()
        {
            laserWallEmitter.EmitProjectile();
        }

        public void DoShockwaveAttack()
        {
            shockwaveEmitter.EmitProjectile();
        }

        public void OnRoll()
        {
            Vector3 direction = this.transform.right;
            float maxDistance = rollDistance + bodyRadius;
            bool directionIsObstructed = Physics.Raycast(this.transform.position, direction, maxDistance, environmentLayers, QueryTriggerInteraction.Ignore);

            if (directionIsObstructed)
            {
                rollDirection = Vector3.zero;
            }
            else
            {
                rollDirection = direction;
            }

            rollDestination = this.transform.position + rollDistance * rollDirection;
            rollAxis = Vector3.Cross(this.transform.up, rollDirection);
        }

        public void RollUpdate()
        {
            // DEBUG
            Debug.DrawLine(this.transform.position, rollDestination, Color.red);

            if (rollDirection == Vector3.zero)
            {
                _animator.SetTrigger(RollEndTrigger);
                return;
            }

            var sqrDistanceToDestination = (rollDestination - this.transform.position).sqrMagnitude;
            if (sqrDistanceToDestination < 0.01)
            {
                _rigidbody.velocity = Vector3.zero;
                this.transform.position = rollDestination;
                _animator.SetTrigger(RollEndTrigger);
            }
            else
            {
                _rigidbody.velocity = rollSpeed * rollDirection;

                float distance = rollSpeed * Time.deltaTime;
                float angle = (distance * 180) / (bodyRadius * Mathf.PI);
                body.RotateAround(body.position, rollAxis, angle);
            }
        }
    }
}