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

        [Tooltip("Angular speed of Shell's rotation to face the player (degree per second).")]
        public float rotationSpeed = 90;

        [Tooltip("Shell's radius when rolling (meters).")]
        public float bodyRadius = 2;

        [Tooltip("Layers of the environment to let Shell detects where to roll.")]
        public LayerMask environmentLayers;

        [Header("Parts")] public Transform body;
        public ProjectileEmitter shockwaveEmitter;
        public ProjectileEmitter laserWallEmitter;

        [Header("References")]
        public PlayerProxy playerProxy;

        // -- Class

        private const string InitializedBool = "IsInitialized";
        private const string LaserWallAttackTrigger = "LaserWallAttackTrigger";
        private const string ShockwaveTrigger = "ShockwaveTrigger";

        private const string RollBeginTrigger = "RollBeginTrigger";
        private const string RollEndTrigger = "RollEndTrigger";


        private Rigidbody _rigidbody;
        private Animator _animator;

        private Vector3 _moveDirection;
        private Vector3 _rollDestination;
        
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
            //int rand = ((int) (Random.value * 10)) % 3;
            //if (rand == 0)
            //{
            //    _animator.SetTrigger(LaserWallAttackTrigger);
            //}
            //else if (rand == 1)
            //{
            //    _animator.SetTrigger(ShockwaveTrigger);
            //}
            //else if (rand == 2)
            //{
            //    _animator.SetTrigger(RollBeginTrigger);
            //}
            
            _animator.SetTrigger(RollBeginTrigger);
        }

        public void IdleUpdate()
        {
            // Rotate towards player
            Vector3 targetDirection = playerProxy.transform.position - this.transform.position;
            Vector3 projectedTargetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
            Quaternion fullRotation = Quaternion.LookRotation(projectedTargetDirection, Vector3.up);

            float maxAngle = rotationSpeed * Time.deltaTime;
            this.transform.rotation = Quaternion.RotateTowards(from: this.transform.rotation, to: fullRotation, maxDegreesDelta: maxAngle);
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
            var rayOrigin = this.transform.position + Vector3.up; // offset the ray origin to avoid detecting the floor
            bool directionIsObstructed = Physics.Raycast(rayOrigin, direction, maxDistance, environmentLayers, QueryTriggerInteraction.Ignore);

            if (directionIsObstructed)
            {
                _moveDirection = Vector3.zero;
                _animator.SetTrigger(RollEndTrigger);
                return;
            }

            _rollDestination = this.transform.position + rollDistance * direction;
            _moveDirection = direction;
        }

        public void RollUpdate()
        {
            // DEBUG
            Debug.DrawLine(this.transform.position, _rollDestination, Color.red);

            if (_moveDirection == Vector3.zero)
            {
                return;
            }

            var direction = _rollDestination - this.transform.position;
            var sqrDistanceToDestination = direction.sqrMagnitude;
            if (sqrDistanceToDestination < 0.05)
            {
                _moveDirection = Vector3.zero;
                _animator.SetTrigger(RollEndTrigger);
            }
            else
            {
                _moveDirection = new Vector3(direction.x, 0, direction.z).normalized;
            }
        }

        void FixedUpdate()
        {
            if (_moveDirection == Vector3.zero)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            _rigidbody.velocity = rollSpeed * _moveDirection;

            float distance = rollSpeed * Time.fixedDeltaTime;
            float angle = (distance * 180) / (bodyRadius * Mathf.PI);

            Vector3 rollAxis = Vector3.Cross(Vector3.up, _rigidbody.velocity);
            body.RotateAround(body.position, rollAxis, angle);
        }
    }
}