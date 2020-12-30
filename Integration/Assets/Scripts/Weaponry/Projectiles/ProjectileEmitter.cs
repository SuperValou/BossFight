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
        [Tooltip("Damage dealt per particle.")]
        public float damagePerParticle = 1;

        [Header("Reference")]
        public ProjectileImpact projectileImpact;
        

        // -- Class

        private const string ProjectileDeflectorTag = "ProjectileDeflector";

        private ParticleSystem _particleSystem;
        private int _particlesPerShot;

        void Start()
        {
            if (projectileImpact == null)
            {
                Debug.LogWarning($"{name} ({this.GetType().Name}) has a null '{nameof(projectileImpact)}'.");
            }

            _particleSystem = this.GetOrThrow<ParticleSystem>();
            var burst = _particleSystem.emission.GetBurst(index: 0);
            _particlesPerShot = (int) burst.count.constant;
        }

        public void EmitProjectile()
        {
            _particleSystem.Emit(_particlesPerShot);
        }

        void OnParticleCollision(GameObject other)
        {
            // Switches
            var activableSwitch = other.GetComponent<ActivationSwitch>();
            if (activableSwitch != null)
            {
                activableSwitch.Flip();
            }
            
            var collisionEvents = _particleSystem.GetCollisionsWith(other);

            // Damages
            var vulnerableCollider = other.GetComponent<VulnerableCollider>();
            if (vulnerableCollider != null)
            {
                float damageAmount = damagePerParticle * collisionEvents.Count;
                DamageData damageData = new DamageData(damageAmount);
                vulnerableCollider.OnHit(damageData, damager: this);
            }
            
            foreach (var collisionEvent in collisionEvents)
            {
                // Projectile deflection
                if (collisionEvent.colliderComponent.tag == ProjectileDeflectorTag)
                {
                    // outVelocity = inVelocity - 2 * (inVelocity dot normalVector) * normalVector
                    Vector3 bouncingVelocity = collisionEvent.velocity -
                                               2 * Vector3.Dot(collisionEvent.velocity, collisionEvent.normal) *
                                               collisionEvent.normal;

                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = collisionEvent.intersection;
                    emitParams.velocity = bouncingVelocity;

                    _particleSystem.Emit(emitParams, count: 1);
                }

                // Projectile impact
                else if (projectileImpact != null)
                {
                    projectileImpact.OccurAt(collisionEvent);
                }
            }
        }
    }
}