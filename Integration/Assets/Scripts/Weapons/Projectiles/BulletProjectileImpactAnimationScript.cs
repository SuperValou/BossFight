using Assets.Scripts.Utilities;
using UnityEngine;

//using DG.Tweening;

namespace Assets.Scripts.Weapons.Projectiles
{
    public class BulletProjectileImpactAnimationScript : ProjectileImpactAnimationScript
    {
        public float attenuationTime = 3;
        
        private MeshRenderer _renderer;
        
        void Awake()
        {
            _renderer = this.GetOrThrow<MeshRenderer>();
            _renderer.enabled = false;
        }

        public override void ImpactInertSurface()
        {
            _renderer.enabled = true;
            
            //_renderer.materials.First().DOFade(0, attenuationTime);

            ImpactDamageable();
        }

        public override void ImpactDamageable()
        {
            DieOut();
        }

        public override void DieOut()
        {
            Invoke(nameof(AutoDestroy), attenuationTime + 1);
        }
        
        private void AutoDestroy()
        {
            Destroy(this.gameObject);
        }
    }
}