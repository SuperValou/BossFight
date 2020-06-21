using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Foes
{
    public class Foe : Damageable
    {
        public GameObject deathAnimation;
        
        void Start()
        {
            
        }
        
        protected override void Die()
        {
            //Instantiate(deathAnimation, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }
    }
}