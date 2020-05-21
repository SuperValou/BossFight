using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Foes
{
    public class Foe : MonoBehaviour
    {
        public float maxHealth = 20;

        public GameObject deathAnimation;

        private NavMeshAgent _navMeshAgent;
        
        public float CurrentHealth { get; private set; }

        public bool IsAlive => CurrentHealth > 0;

        void Start()
        {
            CurrentHealth = maxHealth;
            
            _navMeshAgent = this.GetOrThrow<NavMeshAgent>();
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.SetDestination(Vector3.zero);
            }
        }
        
        public void TakeDamage(float damages)
        {
            CurrentHealth -= damages;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
                Die();
            }
        }

        private void Die()
        {
            Instantiate(deathAnimation, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }

        public void Kill()
        {
            TakeDamage(maxHealth);
        }
    }
}