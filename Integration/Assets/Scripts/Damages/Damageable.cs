using UnityEngine;

namespace Assets.Scripts.Damages
{
    public abstract class Damageable : MonoBehaviour
    {
        public float maxHealth = 20;

        // --

        public float CurrentHealth { get; private set; }

        public bool IsAlive => CurrentHealth > 0;

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(IDamager damager)
        {
            CurrentHealth -= damager.BaseDamage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
        }

        public void InstaKill()
        {
            CurrentHealth = 0;
            Die();
        }

        protected abstract void Die();
    }
}