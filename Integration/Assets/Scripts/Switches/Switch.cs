using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Switches
{
    public class Switch : MonoBehaviour
    {
        // -- Editor

        public bool turnedOnOnStart = false;
        
        // -- Class

        public bool IsTurnedOn { get; private set; }
        public bool IsTurnedOff => !IsTurnedOn;

        void Start()
        {
            IsTurnedOn = turnedOnOnStart;
        }

        void OnCollisionEnter(Collision collision)
        {
            var collidedGameObject = collision.gameObject;
            var projectile = collidedGameObject.GetComponent<Projectile>();

            if (projectile == null)
            {
                return;
            }

            // invert switch state
            IsTurnedOn = !IsTurnedOn;
        }
    }
}