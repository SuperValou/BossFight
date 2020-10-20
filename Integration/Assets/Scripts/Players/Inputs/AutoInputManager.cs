using UnityEngine;

namespace Assets.Scripts.Players.Inputs
{
    public class AutoInputManager : AbstractInputManager
    {
        // -- Editor

        public Vector2 lookVector;

        public float leftRight;
        public float forwardBackward;

        public bool fireButton;

        // -- Class

        private bool _fireButtonHeld = false;

        public override Vector2 GetLookVector()
        {
            return lookVector;
        }

        public override Vector3 GetMoveVector()
        {
            return new Vector3(leftRight, 0, forwardBackward);
        }

        public override bool FireButtonDown()
        {
            if (!fireButton)
            {
                return false;
            }

            if (_fireButtonHeld)
            {
                return false;
            }

            _fireButtonHeld = true;
            return true;
        }

        public override bool FireButton()
        {
            return fireButton;
        }

        public override bool FireButtonUp()
        {
            if (fireButton)
            {
                return false;
            }

            if (!_fireButtonHeld)
            {
                return false;
            }

            _fireButtonHeld = false;
            return true;
        }

        public override bool JumpButton()
        {
            // TODO
            return false;
        }

        public override bool JumpButtonDown()
        {
            // TODO
            return false;
        }

        public override bool BoosterButtonDown()
        {
            // TODO
            return false;
        }

        public override bool DashButtonDown()
        {
            // TODO
            return false;
        }

        public override bool SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection)
        {
            // TODO
            weaponSwitchDirection = WeaponSwitchDirection.None;
            return false;
        }
    }
}