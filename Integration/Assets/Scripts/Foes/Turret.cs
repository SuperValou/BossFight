using System.Collections;
using Assets.Scripts.Damages;
using Assets.Scripts.Environments;
using Assets.Scripts.Players;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes
{
    public class Turret : Damageable
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time between two consecutive volley of bullets (seconds)")]
        public float volleyDelay = 2f;

        [Tooltip("Number of bullets in a volley of shots")]
        public int bulletsPerVolley = 3;

        [Tooltip("Time between two consecutive bullet in a single volley (seconds)")]
        public float bulletDelay = 0.15f;

        [Tooltip("Radius of the disc where bullets will land (meters)")]
        public float imprecisionRadius = 0.5f;

        [Tooltip("How much to aim ahead of the target movement to land a shot (scalar). " +
                 "This should be a tweeked value to approximately match the target's speed and the time for bullets to reach it.")]
        public float targetMovementAnticipation = 15;
        
        [Header("Parts")]
        public Transform pivot;
        public ProjectileEmitter projectileEmitter;
        
        // -- Class

        private Transform _target;
        
        private float _lastVolleyTime;
        private WaitForSeconds _waitForNextBullet;

        private Vector3 _targetLastKnownPosition = Vector3.zero;

        private DamageFeedback _damageFeedback;

        void Start()
        {
            _waitForNextBullet = new WaitForSeconds(bulletDelay);

            _damageFeedback = this.GetOrThrow<DamageFeedback>();
        }

        void OnTriggerEnter(Collider other)
        {
            _target = other?.GetComponent<PlayerProxy>()?.transform;
            
            if (_target != null)
            {
                _targetLastKnownPosition = _target.position;
            }
        }

        void OnTriggerExit(Collider other)
        {
            _target = null;
            _targetLastKnownPosition = Vector3.zero;
        }

        void Update()
        {
            if (_target == null)
            {
                return;
            }
            
            // Aim
            pivot.LookAt(_target);
            
            if (_targetLastKnownPosition == Vector3.zero)
            {
                _targetLastKnownPosition = _target.position;
            }
            
            Vector3 targetExpectedDirection = (_target.position - _targetLastKnownPosition) * targetMovementAnticipation;
            Vector3 predictedTargetPosition = _target.position + targetExpectedDirection;

            Vector3 imprecision = Random.insideUnitCircle * imprecisionRadius; // TODO: Random can't be replayed
            projectileEmitter.transform.LookAt(predictedTargetPosition + imprecision);

            _targetLastKnownPosition = _target.position;


            // Fire
            if (Time.time < _lastVolleyTime + volleyDelay)
            {
                return;
            }

            StartCoroutine(FireRoutine());
            _lastVolleyTime = Time.time;
        }

        private IEnumerator FireRoutine()
        {
            for (int i = 0; i < bulletsPerVolley; i++)
            {
                projectileEmitter.EmitProjectile();
                yield return _waitForNextBullet;
            }
        }

        protected override void OnDamage(VulnerableCollider hitCollider, DamageData damageData, MonoBehaviour damager)
        {
            if (hitCollider.damageMultiplier > 1)
            {
                _damageFeedback.BlinkCritical();
            }
            else
            {
                _damageFeedback.Blink();
            }
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}