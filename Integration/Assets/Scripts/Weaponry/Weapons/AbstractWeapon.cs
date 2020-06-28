using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public abstract class AbstractWeapon : MonoBehaviour
    {
        /// <summary>
        /// What to do when the trigger is pressed and held
        /// </summary>
        public abstract void Fire();

        /// <summary>
        /// What to do when the trigger is released
        /// </summary>
        public abstract void ReleaseFire();
    }
}