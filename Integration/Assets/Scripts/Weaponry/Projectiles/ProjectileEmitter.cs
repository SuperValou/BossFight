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
            var deflector = other.GetComponent<ProjectileDeflector>();
            if (deflector == null && projectileImpact == null)
            {
                return;
            }

            var collisionEvents = _particleSystem.GetCollisionsWith(other);

            foreach (var collisionEvent in collisionEvents)
            {
                if (deflector == null)
                {
                    projectileImpact.OccurAt(collisionEvent);
                }
                else
                {
                    // outVelocity = inVelocity - 2 * (inVelocity dot normalVector) * normalVector
                    Vector3 bouncingVelocity = collisionEvent.velocity - 2 * Vector3.Dot(collisionEvent.velocity, collisionEvent.normal) * collisionEvent.normal;

                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = collisionEvent.intersection;
                    emitParams.velocity = bouncingVelocity;
                    
                    _particleSystem.Emit(emitParams, count: 1);
                }
            }
        }
    }
}