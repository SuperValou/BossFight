using System.Linq;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Parts")]
        public AbstractWeapon[] weapons;

        [Header("References")]
        public AbstractInputManager inputManager;

        // --
        private AbstractWeapon _mainWeapon;

        void Start()
        {
            _mainWeapon = weapons.First();
        }

        void Update()
        {
            if (inputManager.FireButtonDown())
            {
                _mainWeapon.Fire();
            }

            if (inputManager.FireButtonUp())
            {
                _mainWeapon.ReleaseFire();
            }
        }
    }
}