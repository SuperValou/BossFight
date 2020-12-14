using System.Collections;
using Assets.Scripts.Damages;
using Assets.Scripts.Environments;
using Assets.Scripts.Players;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes
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