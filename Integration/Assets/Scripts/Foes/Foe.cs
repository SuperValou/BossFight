using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Foes
{
    public class Foe : Damageable
    {
        // -- Editor

        // -- Class

        private DamageFeedback _damageFeedback;

        void Start()
        {
            _damageFeedback = this.GetOrThrow<DamageFeedback>();
        }

        protected override void OnDamage(VulnerableCollider hitCollider, DamageData damageData, MonoBehaviour damager)
        {
            if (hitCollider.damageMultiplier > 1)
            {
                _damageFeedback.BlinkCritical();
            }
            else
            {
                _damageFeedback.Blink();
            }
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}