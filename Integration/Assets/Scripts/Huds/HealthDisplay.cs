using Assets.Scripts.Damages;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Huds
{
    public class HealthDisplay : MonoBehaviour
    {
        // -- Editor
        
        [Header("Parts")]
        public Slider healthBar;
        public Text healthPointsLabel;
        
        [Header("References")]
        public Damageable player;

        // -- Class
        
        void Start()
        {
            healthBar.minValue = 0;
            healthBar.maxValue = player.maxHealth;
            healthBar.value = player.CurrentHealth;
        }

        void Update()
        {
            healthBar.value = player.CurrentHealth;
            healthPointsLabel.text = player.CurrentHealth.ToString("0");
        }
    }
}