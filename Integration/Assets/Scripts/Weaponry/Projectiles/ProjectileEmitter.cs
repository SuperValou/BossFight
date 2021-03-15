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
            var collisionEvents = _particleSystem.GetCollisionsWith(other);
            
            foreach (var collisionEvent in collisionEvents)
            {
                var otherCollider = collisionEvent.colliderComponent;

                // Switches
                var activableSwitch = otherCollider.GetComponent<ActivationSwitch>();
                if (activableSwitch != null)
                {
                    activableSwitch.Flip();
                }
                
                // Damages
                var vulnerableCollider = otherCollider.GetComponent<VulnerableCollider>();
                if (vulnerableCollider != null)
                {
                    DamageData damageData = new DamageData(damagePerParticle);
                    vulnerableCollider.OnHit(damageData, damager: this);
                }

                // Deflectors
                var deflector = otherCollider.GetComponent<ProjectileDeflector>();
                if (deflector != null)
                {
                    // outVelocity = inVelocity - 2 * (inVelocity dot normalVector) * normalVector
                    Vector3 bouncingVelocity = collisionEvent.velocity - 2 * Vector3.Dot(collisionEvent.velocity, collisionEvent.normal) * collisionEvent.normal;

                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = collisionEvent.intersection;
                    emitParams.velocity = bouncingVelocity;

                    _particleSystem.Emit(emitParams, count: 1);
                }

                // Impacts
                else if (projectileImpact != null)
                {
                    projectileImpact.OccurAt(collisionEvent);
                }
            }
        }
    }
}