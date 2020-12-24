using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Charges
{
    public class WeaponCharge : MonoBehaviour
    {
        // -- Editor

        [Header("Values")] [Tooltip("Time to fully charge (seconds).")]
        public float timeToCharge = 1f;

        [Tooltip("Delay before actually beginning to charge after requested (seconds).")]
        public float beginDelay = 0.1f;

        [Tooltip("Minimum value to reach to be considered a bit charged (between 0 and 1)")] [Range(0, 1)]
        public float minChargeThreshold = 0.1f;

        [Header("References")] public MonoBehaviour[] chargeNotifiables;


        // -- Class

        private readonly ICollection<IChargeNotifiable> _chargeNotifiables = new HashSet<IChargeNotifiable>();

        private float _holdTime;

        /// <summary>
        /// Is it charging?
        /// </summary>
        public bool IsCharging { get; private set; }

        /// <summary>
        /// Has the charge at least reached the lowest threshold?
        /// </summary>
        public bool IsMinimalyCharged => ChargeValue >= minChargeThreshold;

        /// <summary>
        /// Has the charge reached its max value?
        /// </summary>
        public bool IsFullyCharged => ChargeValue >= 1;

        /// <summary>
        /// Charge value between 0 (at rest) and 1 (fully charged).
        /// </summary>
        public float ChargeValue { get; private set; } = 0;

        void Start()
        {
            foreach (var monoBehaviour in chargeNotifiables)
            {
                _chargeNotifiables.Add((IChargeNotifiable) monoBehaviour);
            }
        }

        public void Begin()
        {
            if (IsCharging)
            {
                return;
            }

            _holdTime = Time.time;
        }

        void Update()
        {
            if (timeToCharge <= 0)
            {
                Debug.LogWarning($"{nameof(WeaponCharge)} ({gameObject}): {nameof(timeToCharge)} was not strictly positive, defaulted to 1.");
                timeToCharge = 1;
                return;
            }

            if (_holdTime <= 0)
            {
                return;
            }
            
            // Charge-beginning delay
            if (!IsCharging && Time.time > _holdTime + beginDelay)
            {
                IsCharging = true;

                foreach (var notifiable in _chargeNotifiables)
                {
                    try
                    {
                        notifiable.OnChargeBegin();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                return;
            }
            
            // Update charge value
            float chargingTime = Time.time - _holdTime - beginDelay;
            float previousCharge = ChargeValue;
            ChargeValue = Mathf.Clamp01(chargingTime / timeToCharge);

            // Min threshold notification
            if (previousCharge < minChargeThreshold && ChargeValue >= minChargeThreshold)
            {
                foreach (var notifiable in _chargeNotifiables)
                {
                    try
                    {
                        notifiable.OnMinChargeThresholdReached();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            // Full charge notification
            if (previousCharge < 1 && ChargeValue >= 1)
            {
                foreach (var notifiable in _chargeNotifiables)
                {
                    try
                    {
                        notifiable.OnFullyCharged();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }

        public void Stop()
        {
            IsCharging = false;
            _holdTime = -1;
            
            foreach (var notifiable in _chargeNotifiables)
            {
                try
                {
                    notifiable.OnChargeStop();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        // Will begin a new charge cycle if Stop() was not called beforehand
        public void Clear()
        {
            ChargeValue = 0;
            IsCharging = false;
            
            foreach (var notifiable in _chargeNotifiables)
            {
                try
                {
                    notifiable.OnChargeClear();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}