using Assets.Scripts.Damages;
using Assets.Scripts.Huds;
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
        public HealthDisplay healthDisplay;

        // -- Class
        
        protected override void OnDamageTaken()
        {
            if (healthDisplay != null)
            {
                healthDisplay.Show((Damageable) this);
            }
        }

        protected override void Die()
        {
            if (healthDisplay != null)
            {
                healthDisplay.Show((Damageable) this);
            }

            if (deathAnimation != null)
            {
                Instantiate(deathAnimation, this.transform.position, this.transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}