using UnityEngine;

namespace Assets.Scripts.Damages
{
    public interface IDamageNotifiable
    {
        void OnDamage(Damageable damageable, DamageData damageData, MonoBehaviour damager);
        void OnDeath(Damageable damageable);
    }
}