using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class WeaponCharge : MonoBehaviour
    {
        // -- Editor
        [Tooltip("Time in second to fully charge")]
        public float timeToCharge = 3;
        
        // -- Class
        private float _holdTime;

        public bool IsCharging { get; set; }

        /// <summary>
        /// Charge value between 0 (at rest) and 1 (fully charged).
        /// </summary>
        public float Value { get; private set; } = 0;

        public void Begin()
        {
            if (IsCharging)
            {
                return;
            }

            _holdTime = Time.time;
            IsCharging = true;
        }

        void Update()
        {
            if (IsCharging && timeToCharge > 0)
            {
                float chargingTime = Time.time - _holdTime;
                Value = Mathf.Clamp01(chargingTime / timeToCharge);
            }
        }

        public void Stop()
        {
            IsCharging = false;
        }

        public void Clear()
        {
            Value = 0;
        }
    }
}