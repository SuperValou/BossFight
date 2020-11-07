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
        public AbstractInput input;

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
            if (input.FireButtonDown())
            {
                CurrentWeapon.InitFire();
            }

            if (input.FireButtonUp())
            {
                CurrentWeapon.ReleaseFire();
            }

            if (!input.FireButton() && input.SwitchWeaponDown(out WeaponSwitchDirection direction))
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