using Assets.Scripts.Players.Inputs;
using Assets.Scripts.Players.LockOns;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Players
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        // -- Editor

        [Header("Values - movement")]
        [Tooltip("Speed of the player when moving (m/s).")]
        public float walkSpeed = 10f;

        [Tooltip("Forward speed of the player when dashing (m/s).")]
        public float forwardDashSpeed = 15f;

        [Tooltip("Upward speed of the player when dashing (m/s).")]
        public float upwardDashSpeed = 2f;

        [Tooltip("Forward speed of the player when hitting the booster button (m/s).")]
        public float forwardBoosterSpeed = 4f;

        [Tooltip("Upward speed of the player when hitting the booster button (m/s).")]
        public float upwardBoosterSpeed = 28f;
        
        [Tooltip("Vertical speed of the player when hitting the jump button (m/s).")]
        public float jumpSpeed = 15f;
        
        [Tooltip("Gravity pull applied on the player (m/s²).")]
        public float gravity = 35f;
        
        [Tooltip("Units that player can fall before a falling function is run.")]
        public float fallingThreshold = 10.0f;


        [Header("Values - vision")]
        [Tooltip("How far up can you look? (degrees)")]
        [Range(0, 90)]
        public float maxUpPitchAngle = 60;

        [Tooltip("How far down can you look? (degrees)")]
        [Range(-90, 0)]
        public float maxDownPitchAngle = -60;


        [Header("Parts")]
        public Transform headTransform;
        

        [Header("References")]
        public AbstractInput input;


        [Header("Abilities")]
        public bool hasJumpAbility;
        public bool hasDashAbility;
        public bool hasBoosterAbility;


        // -- Class

        private Transform _transform;
        private CharacterController _controller;
        private WeaponManager _weaponManager;
        private LockOnManager _lockOnManager;

        private bool _isGrounded;

		private bool _canUseBooster;
		private bool _canDash;
		
        private bool _isFalling;		
        private float _fallStartHeigth;
        
        private Vector3 _externalVelocityVector = Vector3.zero; // x is left-right, y is up-down, z is forward-backward
        
        private float _headPitch = 0; // rotation to look up or down


        void Start()
        {
            _transform = this.GetOrThrow<Transform>();
            _controller = this.GetOrThrow<CharacterController>();
            _weaponManager = this.GetOrThrow<WeaponManager>();
            _lockOnManager = this.GetOrThrow<LockOnManager>();
        }


        void Update()
        {
            UpdateMove();
            UpdateFire();
        }

        void LateUpdate()
        {
            UpdateLookAround();
        }
        
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // touched something
        }
        
        private void UpdateLookAround()
        {
            if (input.LockOnButtonDown())
            {
                if (_lockOnManager.HasTargetLocked)
                {
                    _lockOnManager.Unlock();
                }
                else
                {
                    _lockOnManager.TryLockOnTarget();
                }
            }
            
            if (_lockOnManager.HasTargetLocked)
            {
                Transform lockOnTarget = _lockOnManager.GetLockedTarget().transform;

                // body lock-on
                var targetDirectionFromBody = lockOnTarget.position - _transform.position;
                var targetDirectionOnHorizontalPlane = new Vector3(targetDirectionFromBody.x, 0, targetDirectionFromBody.z);

                Quaternion bodyRotation = Quaternion.FromToRotation(_transform.forward, targetDirectionOnHorizontalPlane);
                _transform.Rotate(_transform.up, bodyRotation.eulerAngles.y);

                // head lock-on
                var targetDirectionFromHead = lockOnTarget.position - headTransform.position;
                var targetDirectionOnLocalVerticalZPlane = Vector3.ProjectOnPlane(targetDirectionFromHead, headTransform.right);

                var pitchAngle = Vector3.SignedAngle(headTransform.forward, targetDirectionOnLocalVerticalZPlane, headTransform.right);

                _headPitch = Mathf.Clamp(_headPitch + pitchAngle, maxDownPitchAngle, maxUpPitchAngle);
                headTransform.localRotation = Quaternion.Euler(_headPitch, 0, 0);
            }
            else
            {
                // horizontal look
                Vector2 lookMovement = input.GetLookVector();
                _transform.Rotate(Vector3.up, lookMovement.x);

                // vertical look
                _headPitch = Mathf.Clamp(_headPitch - lookMovement.y, maxDownPitchAngle, maxUpPitchAngle);
                headTransform.localRotation = Quaternion.Euler(_headPitch, 0, 0);
            }
        }

        private void UpdateMove()
        {
			// Movement
            Vector3 localInputDirection = input.GetMoveVector();
            Vector3 globalInputDirection = _transform.TransformDirection(localInputDirection);
            Vector3 inputVelocityVector = globalInputDirection * walkSpeed;
            
            if (_isGrounded)
            {
                _externalVelocityVector = Vector3.zero;
                
                // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
                if (_isFalling)
                {
                    _isFalling = false;
                    if (_transform.position.y < _fallStartHeigth - fallingThreshold)
                    {
                        OnFell(_fallStartHeigth - _transform.position.y);
                    }
                }

				// Reset Booster & Dash
				_canUseBooster = true;
				_canDash = true;
				
                // Jump
				if (hasJumpAbility && input.JumpButtonDown())
				{
					_externalVelocityVector.y = jumpSpeed;			
				}
            }
            else
            {
                // If we stepped over a cliff or something, set the height at which we started falling
                if (!_isFalling)
                {
                    _isFalling = true;
                    _fallStartHeigth = _transform.position.y;
                }

                // Mid-air dash
                if (hasDashAbility && _canDash && input.DashButtonDown())
                {
                    Vector3 dashVelocity;
                    if (globalInputDirection == Vector3.zero)
                    {
                        // If the player is not willing to move in any specific direction, then dash forward
                        dashVelocity = this.transform.forward * forwardDashSpeed;
                    }
                    else
                    {
                        dashVelocity = globalInputDirection.normalized * forwardDashSpeed;
                    }

                    // Jump a little bit when dashing
                    dashVelocity.y = upwardDashSpeed;

                    // Apply dash
                    _externalVelocityVector.x = dashVelocity.x;
                    _externalVelocityVector.y = dashVelocity.y;
                    _externalVelocityVector.z = dashVelocity.z;

                    _canDash = false;
                }
            }

			if (hasBoosterAbility && _canUseBooster && input.BoosterButtonDown())
			{
				_externalVelocityVector.y = upwardBoosterSpeed;
				_externalVelocityVector += this.transform.forward * forwardBoosterSpeed;
			    _canUseBooster = false;
			}
			
			
            
            // Apply gravity
            _externalVelocityVector.y -= gravity * Time.deltaTime;

            // Check ceilling
            if (_controller.collisionFlags.HasFlag(CollisionFlags.Above))
            {
                _externalVelocityVector.y = Mathf.Min(0, _externalVelocityVector.y);
            }

            // Actually move the controller
            Vector3 controllerVelocity = _externalVelocityVector + inputVelocityVector;
            _controller.Move(controllerVelocity * Time.deltaTime);
            _isGrounded = _controller.isGrounded;
        }

        private void UpdateFire()
        {
            if (input.FireButtonDown())
            {
                _weaponManager.InitFire();
            }

            if (input.FireButtonUp())
            {
                _weaponManager.ReleaseFire();
            }

            if (!input.FireButton() && input.SwitchWeaponDown(out WeaponSwitchDirection direction))
            {
                _weaponManager.SwitchWeapon(direction);
            }
        }

        private void OnFell(float fallDistance)
        {
            // fell and touched the ground
        }
    }
}