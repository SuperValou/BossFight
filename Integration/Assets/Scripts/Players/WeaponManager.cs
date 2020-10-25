using System.Linq;
using Assets.Scripts.Players.Inputs;
using Assets.Scripts.Weaponry.Weapons;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class WeaponManager : MonoBehaviour
    {
        // -- Editor

        [Header("Parts")]
        public AbstractWeapon[] weapons;
        
        [Header("References")]
        public AbstractInputManager inputManager;

        // -- Class
        
        private int _currentWeaponIndex;

        public AbstractWeapon CurrentWeapon { get; private set; }

        void Start()
        {
            CurrentWeapon = weapons.First();
            _currentWeaponIndex = 0;
        }

        void Update()
        {
            if (inputManager.FireButtonDown())
            {
                CurrentWeapon.InitFire();
            }

            if (inputManager.FireButtonUp())
            {
                CurrentWeapon.ReleaseFire();
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

            CurrentWeapon = weapons[_currentWeaponIndex];
        }
    }
}