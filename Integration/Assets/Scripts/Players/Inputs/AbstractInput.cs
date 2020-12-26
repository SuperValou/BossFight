using UnityEngine;

namespace Assets.Scripts.Players.Inputs
{
    public abstract class AbstractInput : MonoBehaviour
    {
        /// <summary>
        /// Returns the input vector for looking around.
        /// </summary>
        public abstract Vector2 GetLookVector();

        /// <summary>
        /// Returns the input vector for moving around.
        /// </summary>
        public abstract Vector3 GetMoveVector();

        /// <summary>
        /// Returns true during the frame where the Fire button is pressed.
        /// </summary>
        public abstract bool FireButtonDown();

        /// <summary>
        /// Returns wheter or not the Fire button is held down.
        /// </summary>
        public abstract bool FireButton();

        /// <summary>
        /// Returns true during the frame where the Fire button is released.
        /// </summary>
        public abstract bool FireButtonUp();

        /// <summary>
        /// Returns wheter or not the Jump button is held down.
        /// </summary>
        public abstract bool JumpButton();

        /// <summary>
        /// Returns true during the frame where the Jump button is pressed.
        /// </summary>
        public abstract bool JumpButtonDown();

        /// <summary>
        /// Returns true during the frame where the Booster button is pressed.
        /// </summary>
        public abstract bool BoosterButtonDown();

        /// <summary>
        /// Returns true during the frame where the Dash button is pressed.
        /// </summary>
        public abstract bool DashButtonDown();

        /// <summary>
        /// Returns true during the frame where the Switch Weapon button is pressed.
        /// </summary>
        public abstract bool SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection);

        /// <summary>
        /// Returns true during the frame where the Lock-on button is pressed.
        /// </summary>
        public abstract bool LockOnButtonDown();
    }
}