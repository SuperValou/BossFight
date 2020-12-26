using UnityEngine;

namespace Assets.Scripts.Weaponry.Impacts
{
    public abstract class ProjectileImpact : MonoBehaviour
    {
        public abstract void OccurAt(ParticleCollisionEvent collisionEvent);
    }
}