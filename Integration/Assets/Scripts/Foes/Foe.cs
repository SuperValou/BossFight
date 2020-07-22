using Assets.Scripts.Damages;
using Assets.Scripts.Huds;
using Assets.Scripts.Proxies;
using UnityEngine;

namespace Assets.Scripts.Foes
{
    public class Foe : Damageable
    {
        // -- Editor

        [Header("Parts")]
        public GameObject deathAnimation;
        
        [Header("References")]
        [Tooltip("Can be null")]
        public FoeHealthDisplayProxy foeHealthDisplayProxy;

        // -- Class
        
        protected override void OnDamageTaken()
        {
            if (foeHealthDisplayProxy != null)
            {
                foeHealthDisplayProxy.Show((Damageable) this);
            }
        }

        protected override void Die()
        {
            if (foeHealthDisplayProxy != null)
            {
                foeHealthDisplayProxy.Show((Damageable) this);
            }

            if (deathAnimation != null)
            {
                Instantiate(deathAnimation, this.transform.position, this.transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}