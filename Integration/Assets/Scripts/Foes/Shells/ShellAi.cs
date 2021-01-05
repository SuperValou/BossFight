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

        private Vector3 _selectedRollDirection;
        private Vector3 _rollWorldDestination;
        
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
            
            //_animator.SetTrigger(RollBeginTrigger);
        }

        public void IdleUpdate()
        {
            // Rotate towards player
            Vector3 targetDirection = playerProxy.transform.position - this.transform.position;
            Vector3 projectedTargetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
            //Vector3 projectedForward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
            //float targetAngle = Vector3.SignedAngle(projectedForward, projectedTargetDirection, Vector3.up);

            float maxAngle = rotationSpeed * Time.deltaTime;
            //float angle = Mathf.Sign(targetAngle) * Mathf.Clamp(Mathf.Abs(targetAngle), 0, maxAngle);

            Quaternion fullRotation = Quaternion.LookRotation(projectedTargetDirection, Vector3.up);

            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, fullRotation, maxAngle);
            
            //this.transform.Rotate(Vector3.up, angle);
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
                _selectedRollDirection = Vector3.zero;
                _animator.SetTrigger(RollEndTrigger);
                return;
            }

            _selectedRollDirection = direction;
            _rollWorldDestination = this.transform.position + rollDistance * _selectedRollDirection;
        }

        public void RollUpdate()
        {
            // DEBUG
            Debug.DrawLine(this.transform.position, _rollWorldDestination, Color.red);

            if (_selectedRollDirection == Vector3.zero)
            {
                return;
            }

            var sqrDistanceToDestination = (_rollWorldDestination - this.transform.position).sqrMagnitude;
            if (sqrDistanceToDestination < 0.05)
            {
                _rigidbody.velocity = Vector3.zero;
                this.transform.position = _rollWorldDestination;
                _animator.SetTrigger(RollEndTrigger);
            }
            else
            {
                Vector3 localDestination = _rollWorldDestination - this.transform.position;
                Vector3 projectedDirection = new Vector3(localDestination.x, 0, localDestination.z).normalized;
                _rigidbody.velocity = rollSpeed * projectedDirection;

                float distance = rollSpeed * Time.deltaTime;
                float angle = (distance * 180) / (bodyRadius * Mathf.PI);
                body.RotateAround(body.position, -1 * body.forward, angle);
            }
        }

        
    }
}