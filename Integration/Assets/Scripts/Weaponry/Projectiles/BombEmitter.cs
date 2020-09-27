using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BombEmitter : MonoBehaviour
    {
        // -- Editor
        
        public BombImpact bombImpact;
        
        // -- Class

        private ParticleSystem _particleSystem;
        
        void Start()
        {
            if (bombImpact == null)
            {
                Debug.LogWarning($"{this.GetType().Name} ({name}) has a null '{nameof(bombImpact)}'.");
            }

            _particleSystem = this.GetOrThrow<ParticleSystem>();
        }

        public void EmitProjectile()
        {
            _particleSystem.Emit(1);
        }

        void OnParticleCollision(GameObject other)
        {
            if (bombImpact == null)
            {
                return;
            }

            var collisionEvents = _particleSystem.GetCollisionsWith(other);

            foreach (var collisionEvent in collisionEvents)
            {
                bombImpact.OccurAt(collisionEvent);
            }
        }
    }
}