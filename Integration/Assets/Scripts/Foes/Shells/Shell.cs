using Assets.Scripts.Damages;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes.Shells
{
    public class Shell : Damageable
    {
        void Start()
        {
            // do nothing
        }

        protected override void OnDamage(VulnerableCollider hitCollider, DamageData damageData, MonoBehaviour damager)
        {
            // ouch
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}