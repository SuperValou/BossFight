using System;
using System.Collections.Generic;
using Assets.Scripts.Weaponry.Charges;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class WeaponCharge : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time to fully charge (seconds).")]
        public float timeToCharge = 1f;

        [Tooltip("Minimum value ")]
        public float minChargeThreshold = 0.1f;

        [Header("References")]
        public MonoBehaviour[] chargeNotifiables;


        // -- Class

        private float _holdTime;
        private ICollection<IChargeNotifiable> _chargeNotifiables = new HashSet<IChargeNotifiable>();

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
        }

        void Update()
        {
            if (timeToCharge <= 0)
            {
                Debug.LogWarning($"{nameof(WeaponCharge)} ({gameObject}): {nameof(timeToCharge)} was not strictly positive, defaulted to 1.");
                timeToCharge = 1;
                return;
            }

            if (!IsCharging)
            {
                return;
            }

            float chargingTime = Time.time - _holdTime;
            float previousCharge = ChargeValue;
            ChargeValue = Mathf.Clamp01(chargingTime / timeToCharge);

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

        public void Clear()
        {
            _holdTime = Time.time;
            ChargeValue = 0;

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