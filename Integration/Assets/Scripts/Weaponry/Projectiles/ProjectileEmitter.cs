using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ProjectileEmitter : MonoBehaviour
    {
        // -- Editor
        
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