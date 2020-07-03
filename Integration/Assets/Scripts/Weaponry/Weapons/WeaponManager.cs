using System.Linq;
using System.Net;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        // -- Editor

        [Header("Parts")]
        public AbstractWeapon[] weapons;

        [Header("UI")]
        public Text text;

        [Header("References")]
        public AbstractInputManager inputManager;

        // -- Class

        private AbstractWeapon _currentWeapon;
        private int _currentWeaponIndex;

        void Start()
        {
            _currentWeapon = weapons.First();
            _currentWeaponIndex = 0;
        }

        void Update()
        {
            if (inputManager.FireButtonDown())
            {
                _currentWeapon.InitFire();
            }

            if (inputManager.FireButtonUp())
            {
                _currentWeapon.ReleaseFire();
            }

            if (!inputManager.FireButton() && inputManager.SwitchWeaponDown(out WeaponSwitchDirection direction))
            {
                if (direction == WeaponSwitchDirection.Next)
                {
                    _currentWeaponIndex = (_currentWeaponIndex + 1) % weapons.Length;
                    
                }
                else if (direction == WeaponSwitchDirection.Previous)
                {
                    _currentWeaponIndex = (_currentWeaponIndex + weapons.Length -1) % weapons.Length;
                }
            }

            _currentWeapon = weapons[_currentWeaponIndex];
            text.text = _currentWeapon.name;
        }
    }
}