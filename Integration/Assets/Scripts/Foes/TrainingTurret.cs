using System.Collections;
using Assets.Scripts.Environments;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes
{
    public class TrainingTurret : MonoBehaviour
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
        public Transform cannon;
        public ProjectileEmitter projectileEmitter;
        public ActivationSwitch activationActivationSwitch;

        [Header("References")]
        public Transform target;

        // -- Class

        private float _lastVolleyTime;
        private WaitForSeconds _waitForNextBullet;

        private Vector3 _targetLastKnownPosition = Vector3.zero;

        void Start()
        {
            if (target == null)
            {
                Debug.LogWarning($"{this.GetType().Name} ({name}) has a null {nameof(target)}.");
            }

            _waitForNextBullet = new WaitForSeconds(bulletDelay);
        }

        void Update()
        {
            if (target == null)
            {
                return;
            }

            if (activationActivationSwitch.IsTurnedOff)
            {
                _targetLastKnownPosition = Vector3.zero;
                _lastVolleyTime = Time.time;
                return;
            }
            
            // Aim

            cannon.LookAt(target);
            
            if (_targetLastKnownPosition == Vector3.zero)
            {
                _targetLastKnownPosition = target.position;
            }
            
            Vector3 targetExpectedDirection = (target.position - _targetLastKnownPosition) * targetMovementAnticipation;
            Vector3 predictedTargetPosition = target.position + targetExpectedDirection;

            Vector3 imprecision = Random.insideUnitCircle * imprecisionRadius; // TODO: Random can't be replayed
            projectileEmitter.transform.LookAt(predictedTargetPosition + imprecision);

            _targetLastKnownPosition = target.position;


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
    }
}