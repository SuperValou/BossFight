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
        private const float DistanceToDestionTolerance = 0.5f; // avoid getting stuck and 'vibrating' when getting close to the roll destination

        private Rigidbody _rigidbody;
        private Animator _animator;
        private float _rollDistance;

        private Vector3 _velocityVector;
        private Vector3 _rollDestination;
        private Vector3 _bodyRollAxis;
        private Quaternion _bodyInitialLocalRotation;

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
            int rand = ((int)(Random.value * 10)) % 3;
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
            // choose a direction to roll to
            Vector3[] directions = {
                this.transform.forward, // front
                this.transform.right, // right
                -1 * this.transform.right, // left
                -1 * this.transform.forward // back
            };

            int randomStartIndex = Mathf.FloorToInt((Random.value - float.Epsilon) * directions.Length);

            Vector3 selectedDirection = Vector3.zero;
            for (int i = 0; i < directions.Length; i++)
            {
                int index = (i + randomStartIndex) % directions.Length;
                Vector3 currentDirection = directions[index];

                selectedDirection = Vector3.ProjectOnPlane(currentDirection, Vector3.up).normalized;
                if (DirectionIsObstructed(selectedDirection))
                {
                    selectedDirection = Vector3.zero;
                    continue;
                }

                // found a direction to roll to
                break;
            }
            
            if (selectedDirection == Vector3.zero)
            {
                // all directions are obstructed
                _animator.SetTrigger(RollEndTrigger);
                return;
            }

            // Compute destination and roll axis
            _rollDestination = this.transform.position + _rollDistance * selectedDirection;

            Vector3 worldRollAxis = Vector3.Cross(Vector3.down, selectedDirection);
            _bodyRollAxis = body.InverseTransformVector(worldRollAxis);
            _bodyInitialLocalRotation = body.localRotation;

            // initiate movement
            _velocityVector = selectedDirection * rollSpeed;
        }

        public void RollUpdate()
        {
            if (_velocityVector == Vector3.zero)
            {
                return;
            }

            // check if we're close to destination and update direction
            var destinationDirection = _rollDestination - this.transform.position;
            var distanceToDestination = destinationDirection.magnitude;
            if (distanceToDestination < DistanceToDestionTolerance)
            {
                _velocityVector = Vector3.zero;
                _animator.SetTrigger(RollEndTrigger);
                body.localRotation = _bodyInitialLocalRotation;
                return;
            }
            
            _velocityVector = destinationDirection.normalized * rollSpeed;

            // rotate body
            float coveredDistanceRatio = distanceToDestination / _rollDistance;
            body.localRotation = _bodyInitialLocalRotation * Quaternion.AngleAxis(coveredDistanceRatio * 360, _bodyRollAxis);
            
            // DEBUG
            Debug.DrawLine(this.transform.position, _rollDestination, Color.red);
        }

        void FixedUpdate()
        {
            _rigidbody.velocity = _velocityVector;
        }

        private bool DirectionIsObstructed(Vector3 direction)
        {
            float checkRadius = bodyRadius + CollisionCheckSafetyMargin; // avoid getting stuck on tangent wall
            Vector3 checkOffset = 2 * CollisionCheckSafetyMargin * Vector3.up; // offset a bit up to avoid detecting the floor (take margin on radius into account)
            Vector3 checkStart = this.transform.position + bodyRadius * Vector3.up + checkOffset; // transform.position is on the ground, so add radius to be at center of mass
            float checkDistance = _rollDistance + CollisionCheckSafetyMargin; // avoid attempting to set a destination too close to a wall and not being able to physically reach it
            Vector3 checkEnd = checkStart + checkDistance * direction;

            bool directionIsObstructed = Physics.CheckCapsule(checkStart, checkEnd, checkRadius, environmentLayers, QueryTriggerInteraction.Ignore);
            return directionIsObstructed;
        }
    }
}