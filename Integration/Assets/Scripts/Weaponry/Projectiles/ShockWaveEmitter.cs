using System;
using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    public class ShockWaveEmitter : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        public int particlesToEmit = 30;
        public int damagePerParticle = 1;

        // -- Class

        private ParticleSystem _particleSystem;
        
        void Start()
        {
            _particleSystem = this.GetOrThrow<ParticleSystem>();
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

            float damageAmount = damagePerParticle * collisionEvents.Count;
            DamageData damageData = new DamageData(damageAmount);
            damageable.TakeDamage(damageData, damager: this);
        }

        public void EmitShockWave()
        {
            _particleSystem.Emit(particlesToEmit);
        }
    }
}