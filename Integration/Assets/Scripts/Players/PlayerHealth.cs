using Assets.Scripts.Damages;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerHealth : Damageable
    {
        protected override void OnDamageTaken()
        {
            // TODO: say ouch
        }

        protected override void Die()
        {
            // TODO: game over
            Debug.LogWarning("Game over");
        }
    }
}