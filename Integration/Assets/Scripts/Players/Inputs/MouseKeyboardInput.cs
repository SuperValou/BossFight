using UnityEngine;

namespace Assets.Scripts.Players.Inputs
{
    public class MouseKeyboardInput : AbstractInput
    {
        // -- Editor

        [Header("Mouse")]
        public CursorLockMode cursorLockMode = CursorLockMode.Locked;
        public float mouseSensitivity = 12f;
        public float scrollSensitivity = 1f;

        [Header("Keyboard")]
        public float keyboardSensitivity = 1f;

        // -- Class

        // Mouse
        private const string MouseHorizontalAxisName = "Mouse X";
        private const string MouseVerticalAxisName = "Mouse Y";
        private const int MouseFireButton = 0;
        private const int MouseLockOnButton = 1;

        // Keyboard
        private const string KeyboardHorizontalAxisName = "Horizontal";
        private const string KeyboardVerticalAxisName = "Vertical";
        
        private readonly KeyCode _keyboardJumpButtonKey = KeyCode.Space;
        private readonly KeyCode _keyboardBoosterButtonKey = KeyCode.LeftShift;
        
        void Start()
        {
            Cursor.lockState = cursorLockMode;
        }

        void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public override Vector2 GetLookVector()
        {
            float x = Input.GetAxis(MouseHorizontalAxisName) * mouseSensitivity * Time.deltaTime;
            float y = Input.GetAxis(MouseVerticalAxisName) * mouseSensitivity * Time.deltaTime;

            Vector2 mouseMovement = new Vector2(x, y);
            
            return mouseMovement * mouseSensitivity;
        }

        public override Vector3 GetMoveVector()
        {
            float leftRight = Input.GetAxis(KeyboardHorizontalAxisName);
            float forwardBackward = Input.GetAxis(KeyboardVerticalAxisName);

            return keyboardSensitivity * new Vector3(x: leftRight, y: 0, z: forwardBackward);
        }

        public override bool FireButtonDown()
        {
            return Input.GetMouseButtonDown(MouseFireButton);
        }
        
        public override bool FireButton()
        {
            return Input.GetMouseButton(MouseFireButton);
        }

        public override bool FireButtonUp()
        {
            return Input.GetMouseButtonUp(MouseFireButton);
        }

        public override bool JumpButton()
        {
            return Input.GetKeyDown(_keyboardJumpButtonKey);
        }

        public override bool JumpButtonDown()
        {
            return Input.GetKeyDown(_keyboardJumpButtonKey);
        }

        public override bool BoosterButtonDown()
        {
            return Input.GetKeyDown(_keyboardBoosterButtonKey);
        }

        public override bool DashButtonDown()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public override bool SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection)
        {
            float scroll = Input.mouseScrollDelta.y * scrollSensitivity;
            if (scroll >= 1)
            {
                weaponSwitchDirection = WeaponSwitchDirection.Next;
                return true;
            }

            if (scroll <= -1)
            {
                weaponSwitchDirection = WeaponSwitchDirection.Previous;
                return true;
            }

            weaponSwitchDirection = WeaponSwitchDirection.None;
            return false;
        }

        public override bool LockOnButtonDown()
        {
            return Input.GetMouseButtonDown(MouseLockOnButton);
        }
    }
}