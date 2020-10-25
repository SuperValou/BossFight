using UnityEngine;

namespace Assets.Scripts.Damages
{
    public abstract class Damageable : MonoBehaviour
    {
        // -- Editor

        public float maxHealth = 20;
        public bool isInvulnerable = false;

        // -- Class

        public float CurrentHealth { get; private set; }

        public bool IsAlive => CurrentHealth > 0;

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(IDamager damager)
        {
            if (!IsAlive)
            {
                return;
            }

            if (isInvulnerable)
            {
                return;
            }

            CurrentHealth -= damager.BaseDamage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
            else
            {
                OnDamageTaken();
            }
        }
        
        public void InstaKill()
        {
            CurrentHealth = 0;
            Die();
        }

        protected abstract void OnDamageTaken();

        protected abstract void Die();
    }
}