using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Foes.Turrets
{
    public class Turret : Damageable
    {
        // -- Editor
         

        // -- Class
        
        private DamageFeedback _damageFeedback;
        private TurretAi _ai;

        void Start()
        {
            _damageFeedback = this.GetOrThrow<DamageFeedback>();
            _ai = this.GetOrThrow<TurretAi>();
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

            _ai.OnGettingAttacked();
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}