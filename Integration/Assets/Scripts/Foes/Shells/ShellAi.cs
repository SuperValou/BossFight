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

        [Header("Values")] 
		[Tooltip("Speed of movement while rolling (m/s).")]
        public float rollSpeed = 5;
        
        [Tooltip("Angular speed of Shell's rotation to face the player (degree per second).")]
        public float rotationSpeed = 90;

        [Tooltip("Shell's radius when rolling (meters).")]
        public float bodyRadius = 2;

        [Tooltip("Layers of the environment to let Shell detects where to roll.")]
        public LayerMask environmentLayers;

        [Header("Parts")] 
		public Transform body;
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

        private const float CollisionCheckSafetyMargin = 0.25f;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private float _rollDistance;

        private Vector3 _moveDirection;
        private Vector3 _rollDestination;
        private Vector3 _rollAxis;
        private Quaternion _rollStartRotation;

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

            _rollDistance = 2 * bodyRadius * Mathf.PI;
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
            // TODO: crap
            Vector3 direction = Vector3.ProjectOnPlane(Vector3.Cross(Vector3.up, playerProxy.transform.position - this.transform.position), Vector3.up).normalized;

            float checkRadius = bodyRadius + CollisionCheckSafetyMargin; // avoid getting stuck on tangent wall
            Vector3 checkOffset = 2 * CollisionCheckSafetyMargin * Vector3.up; // offset a bit up to avoid detecting the floor (take margin on radius into account)
            Vector3 checkStart = this.transform.position + Vector3.up * bodyRadius + checkOffset;
            float checkDistance = _rollDistance + CollisionCheckSafetyMargin; // avoid attempting to set a destination too close to a wall and not being able to physically reach it
            Vector3 checkEnd = checkStart + checkDistance * direction;
            
            bool directionIsObstructed = Physics.CheckCapsule(checkStart, checkEnd, checkRadius, environmentLayers, QueryTriggerInteraction.Ignore);

            if (directionIsObstructed)
            {
                _moveDirection = Vector3.zero;
                _animator.SetTrigger(RollEndTrigger);
                return;
            }

            _moveDirection = direction;
            _rollDestination = this.transform.position + _rollDistance * _moveDirection;

            Vector3 worldRollAxis = Vector3.Cross(Vector3.down, _moveDirection);
            _rollAxis = body.InverseTransformVector(worldRollAxis);
            _rollStartRotation = body.localRotation;
        }

        public void RollUpdate()
        {
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
                body.localRotation = _rollStartRotation;
                return;
            }
            
            _moveDirection = new Vector3(direction.x, 0, direction.z).normalized;

            // rotate body
            float coveredDistanceRatio = direction.magnitude / _rollDistance;

            //body.rotation = _rollStartRotation * Quaternion.AngleAxis(coveredDistanceRatio * 360, _rollAxis);
            Debug.DrawRay(body.position, _rollAxis * 10, Color.blue);
            
            body.localRotation = _rollStartRotation * Quaternion.AngleAxis(coveredDistanceRatio * 360, _rollAxis);
            
            // DEBUG
            Debug.DrawLine(this.transform.position, _rollDestination, Color.red);
        }

        void FixedUpdate()
        {
            if (_moveDirection == Vector3.zero)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            _rigidbody.velocity = rollSpeed * _moveDirection;
        }
    }
}