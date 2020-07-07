using System;
using Assets.Scripts.Damages;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Huds
{
    public class FoeHealthDisplay : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Amount of seconds the display stays up")]
        public float maxDisplayTime = 5;

        [Header("Parts")]
        public Slider healthBar;
        public Text healthPointsLabel;
        public Text damageableNameLabel;
        
        // -- Class

        private Damageable _currentDamageable;

        private bool _uiIsEnabled = false;
        private float _displayTime = 0;

        void Start()
        {
            healthBar.minValue = 0;
            Hide();
        }

        void Update()
        {
            if (!_uiIsEnabled)
            {
                return;
            }

            if (_currentDamageable == null || !_currentDamageable.IsAlive
                || Time.time > _displayTime + maxDisplayTime)
            {
                Hide();
                return;
            }

            healthBar.value = _currentDamageable.CurrentHealth;
            healthPointsLabel.text = _currentDamageable.CurrentHealth.ToString("0");
        }

        public void Show(Damageable damageable)
        {
            if (damageable == null)
            {
                throw new ArgumentNullException(nameof(damageable));
            }
            
            _displayTime = Time.time; // reset display time

            if (_currentDamageable == damageable && _uiIsEnabled)
            {
                return;
            }

            _currentDamageable = damageable;


            healthBar.gameObject.SetActive(true);
            healthBar.maxValue = damageable.maxHealth;
            healthBar.value = damageable.CurrentHealth;

            healthPointsLabel.enabled = true;
            
            damageableNameLabel.enabled = true;
            damageableNameLabel.text = damageable.name;

            _uiIsEnabled = true;
        }

        public void Hide()
        {
            healthBar.gameObject.SetActive(false);
            healthPointsLabel.enabled = false;
            damageableNameLabel.enabled = false;
            _uiIsEnabled = false;
        }
    }
}