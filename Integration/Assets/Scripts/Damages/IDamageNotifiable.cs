using UnityEngine;

namespace Assets.Scripts.Damages
{
    public interface IDamageNotifiable
    {
        void OnDamageNotification(Damageable damageable, DamageData damageData, MonoBehaviour damager);
        void OnDeathNotification(Damageable damageable);
    }
}