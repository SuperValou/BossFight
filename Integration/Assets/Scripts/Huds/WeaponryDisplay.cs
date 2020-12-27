using Assets.Scripts.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Huds
{
    public class WeaponryDisplay : MonoBehaviour
    {
        [Header("Parts")]
        public Text currentWeaponLabel;

        [Header("References")]
        public WeaponManager weaponManager;

        void Update()
        {
            currentWeaponLabel.text = weaponManager.CurrentWeapon.DisplayName;
        }
    }
}