using Assets.Scripts.Damages;
using Assets.Scripts.Environments;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Impacts;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ProjectileEmitter : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        public float baseDamage = 1;

        [Header("Reference")]
        public ProjectileImpact projectileImpact;
        

        // -- Class

        private ParticleSystem _particleSystem;
        
        void Start()
        {
            if (projectileImpact == null)
            {
                Debug.LogWarning($"{this.GetType().Name} ({name}) has a null '{nameof(projectileImpact)}'.");
            }

            _particleSystem = this.GetOrThrow<ParticleSystem>();
        }

        public void EmitProjectile()
        {
            _particleSystem.Emit(1);
        }

        void OnParticleCollision(GameObject other)
        {
            // Switches
            var activableSwitch = other.GetComponent<ActivationSwitch>();
            if (activableSwitch != null)
            {
                activableSwitch.Flip();
            }

            // Damageables
            var vulnerableCollider = other.GetComponent<VulnerableCollider>();
            if (vulnerableCollider != null)
            {
                DamageData damageData = new DamageData(baseDamage);
                vulnerableCollider.OnHit(damageData, damager: this);
            }
            
            // Impact
            if (projectileImpact == null)
            {
                return;
            }

            var collisionEvents = _particleSystem.GetCollisionsWith(other);

            foreach (var collisionEvent in collisionEvents)
            {
                projectileImpact.OccurAt(collisionEvent);
            }
        }
    }
}