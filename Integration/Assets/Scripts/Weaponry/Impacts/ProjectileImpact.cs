using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    public abstract class ProjectileImpact : MonoBehaviour
    {
        public abstract void OccurAt(ParticleCollisionEvent collisionEvent);
    }
}