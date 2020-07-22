using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Damages
{
    public class DamagingParticleSystem : MonoBehaviour, IDamager
    {
        // -- Editor

        [Header("Values")]
        public int damagePerParticle = 2;

        // -- Class

        private ParticleSystem _particleSystem;

        private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

        public float BaseDamage { get; private set; }

        void Start()
        {
            _particleSystem = this.GetOrThrow<ParticleSystem>();
            BaseDamage = damagePerParticle;
        }

        void OnParticleCollision(GameObject collidingGameObject)
        {
            if (collidingGameObject == null)
            {
                throw new ArgumentNullException(nameof(collidingGameObject));
            }

            var damageable = collidingGameObject.GetComponent<Damageable>();
            if (damageable == null)
            {
                return;
            }

            var collisionEvents = _particleSystem.GetCollisionsWith(collidingGameObject);

            foreach (var particleCollisionEvent in collisionEvents)
            {
                //var collisionPoint = particleCollisionEvent.intersection;
                //var impactVelocity = particleCollisionEvent.velocity;
                //var impactNormal = particleCollisionEvent.normal;

                damageable.TakeDamage(this);
            }
        }

        public void Execute()
        {
            _particleSystem.Emit(300);
        }


        
    }
}