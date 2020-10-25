using System;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.Environments.PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        private bool _pickedUp = false;
        
        void OnTriggerEnter(Collider otherCollider)
        {
            if (_pickedUp)
            {
                return;
            }

            if (otherCollider == null)
            {
                throw new ArgumentNullException(nameof(otherCollider));
            }

            var player = otherCollider.GetComponent<Player>();
            ApplyPowerUp(player);

            _pickedUp = true;
            Destroy(this.gameObject);
        }

        protected abstract void ApplyPowerUp(Player player);
    }
}