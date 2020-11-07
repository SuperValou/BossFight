using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public abstract class AbstractWeapon : MonoBehaviour
    {
        public string DisplayName => this.GetType().Name;

        /// <summary>
        /// What to do when the trigger is pressed and held
        /// </summary>
        public abstract void InitFire();

        /// <summary>
        /// What to do when the trigger is released
        /// </summary>
        public abstract void ReleaseFire();
    }
}