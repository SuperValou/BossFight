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

        [Tooltip("Shell's radius when rolling (meters).")]
        public float bodyRadius = 2;

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

        private Rigidbody _rigidbody;
        private Animator _animator;
        
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
            int rand = ((int) (Random.value * 10)) % 2;
            if (rand == 0)
            {
                _animator.SetTrigger(LaserWallAttackTrigger);
            }
            else
            {
                _animator.SetTrigger(ShockwaveTrigger);
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

        public void RollUpdate()
        {
            //float accelerationValue = rollSpeed / Time.deltaTime;
            //Vector3 accelerationVector = this.transform.forward * accelerationValue;
            //Vector3 force = accelerationVector * _rigidbody.mass; // F = m.a
            //_rigidbody.AddForce(force);
            _rigidbody.velocity = rollSpeed * this.transform.forward;

            float distance = rollSpeed * Time.deltaTime;
            float angle = (distance * 180) / (bodyRadius * Mathf.PI);
            body.RotateAround(body.position, body.right, angle);
        }

    }
}