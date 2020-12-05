using System;
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

        public void TakeDamage(VulnerableCollider vulnerableCollider, DamageData damageData, MonoBehaviour damager)
        {
            if (!IsAlive)
            {
                return;
            }

            if (isInvulnerable)
            {
                return;
            }

            CurrentHealth -= damageData.Amount;
            if (CurrentHealth > 0)
            {
                OnDamage(vulnerableCollider, damageData, damager);

                foreach (var damageNotifiable in _damageNotifiables)
                {
                    try
                    {
                        damageNotifiable.OnDamageNotification(this, damageData, damager);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
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

        protected abstract void OnDamage(VulnerableCollider hitCollider, DamageData damageData, MonoBehaviour damager);
        protected abstract void OnDeath();

        private void Die()
        {
            CurrentHealth = 0;
            
            foreach (var damageNotifiable in _damageNotifiables)
            {
                try
                {
                    damageNotifiable.OnDeathNotification(this);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            OnDeath();
        }
    }
}