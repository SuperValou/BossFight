using Assets.Scripts.Damages;
using Assets.Scripts.Environments;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ProjectileEmitter : MonoBehaviour, IDamager
    {
        // -- Editor

        [Header("Values")]
        public float baseDamage = 1;

        [Header("Reference")]
        public ProjectileImpact projectileImpact;
        

        // -- Class

        private ParticleSystem _particleSystem;

        public float BaseDamage { get; private set; }

        void Start()
        {
            if (projectileImpact == null)
            {
                Debug.LogWarning($"{this.GetType().Name} ({name}) has a null '{nameof(projectileImpact)}'.");
            }

            _particleSystem = this.GetOrThrow<ParticleSystem>();
            BaseDamage = baseDamage;
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
            var damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damager: this);
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