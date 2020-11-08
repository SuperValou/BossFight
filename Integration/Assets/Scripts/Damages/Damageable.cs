using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Damages
{
    public abstract class Damageable : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Maximum amount of damage that can be taken before dying.")]
        public float maxHealth = 20;

        [Tooltip("If enabled, damage will be ignored. Usefull to temporarily recover from an attack, or just prevent death.")]
        public bool isInvulnerable = false;

        [Tooltip(nameof(IDamageNotifiable) + " that should be notified when damages are received.")]
        public MonoBehaviour[] onDamageReceived;

        // -- Class

        private readonly ICollection<IDamageNotifiable> _damageNotifiables = new HashSet<IDamageNotifiable>();

        public float CurrentHealth { get; private set; }

        public bool IsAlive => CurrentHealth > 0;

        void Awake()
        {
            CurrentHealth = maxHealth;

            foreach (var monoBehaviour in onDamageReceived)
            {
                _damageNotifiables.Add((IDamageNotifiable) monoBehaviour);
            }
        }

        public void TakeDamage(DamageData damageData, MonoBehaviour damager)
        {
            if (!IsAlive)
            {
                return;
            }

            if (isInvulnerable)
            {
                return;
            }

            CurrentHealth -= damageData.BaseDamage;
            if (CurrentHealth > 0)
            {
                OnDamage(damageData, damager);

                foreach (var damageNotifiable in _damageNotifiables)
                {
                    damageNotifiable.OnDamage(this, damageData, damager);
                }
            }
            else
            {
                Die();
            }
        }
        
        public void InstaKill()
        {
            if (!IsAlive)
            {
                return;
            }

            CurrentHealth = 0;
            Die();
        }

        protected abstract void OnDamage(DamageData damageData, MonoBehaviour damager);
        protected abstract void OnDeath();

        private void Die()
        {
            CurrentHealth = 0;

            OnDeath();

            foreach (var damageNotifiable in _damageNotifiables)
            {
                damageNotifiable.OnDeath(this);
            }
        }
    }
}