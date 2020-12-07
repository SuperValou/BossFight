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
        
        // -- Class
        
        private int _currentWeaponIndex;

        public AbstractWeapon CurrentWeapon { get; private set; }

        void Start()
        {
            CurrentWeapon = weapons.First();
            _currentWeaponIndex = 0;
        }

        public void InitFire()
        {
            CurrentWeapon.InitFire();
        }

        public void ReleaseFire()
        {
            CurrentWeapon.ReleaseFire();
        }

        public void SwitchWeapon(WeaponSwitchDirection direction)
        {
            if (direction == WeaponSwitchDirection.Next)
            {
                _currentWeaponIndex = (_currentWeaponIndex + 1) % weapons.Length;

            }
            else if (direction == WeaponSwitchDirection.Previous)
            {
                _currentWeaponIndex = (_currentWeaponIndex + weapons.Length - 1) % weapons.Length;
            }
            
            CurrentWeapon = weapons[_currentWeaponIndex];
        }
    }
}