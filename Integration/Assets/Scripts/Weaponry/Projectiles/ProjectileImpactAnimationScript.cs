using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    public abstract class ProjectileImpactAnimationScript : MonoBehaviour
    {
        public abstract void ImpactInertSurface();
        public abstract void ImpactDamageable();
        public abstract void DieOut();
    }
}