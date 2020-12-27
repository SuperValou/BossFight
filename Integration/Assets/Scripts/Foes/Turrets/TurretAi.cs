using System.Collections;
using Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking;
using Assets.Scripts.Players;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes.Turrets
{
    public class TurretAi : TargetTrackingAi
    {
        // -- Editor

        [Header("Values")] [Tooltip("Time between two consecutive volley of bullets (seconds)")]
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


        [Header("Parts")] public Transform pivot;
        public ProjectileEmitter projectileEmitter;

        [Header("References")] public PlayerProxy playerProxy;


        // -- Class

        private WaitForSeconds _waitForNextBullet;
        private float _lastVolleyTime;

        private Vector3 _targetLastKnownPosition = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            _waitForNextBullet = new WaitForSeconds(bulletDelay);

            Target = playerProxy.transform;
        }

        public override void AlertUpdate()
        {
            base.AlertUpdate();
            if (Target == null)
            {
                return;
            }

            pivot.LookAt(Target);
        }

        public override void HostileUpdate()
        {
            base.HostileUpdate();
            if (Target == null)
            {
                return;
            }

            // Aim
            pivot.LookAt(Target);

            if (_targetLastKnownPosition == Vector3.zero)
            {
                _targetLastKnownPosition = Target.position;
            }

            Vector3 targetExpectedDirection = (Target.position - _targetLastKnownPosition) * targetMovementAnticipation;
            Vector3 predictedTargetPosition = Target.position + targetExpectedDirection;

            Vector3 imprecision = Random.insideUnitCircle * imprecisionRadius; // TODO: Random can't be replayed
            projectileEmitter.transform.LookAt(predictedTargetPosition + imprecision);

            _targetLastKnownPosition = Target.position;


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