using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Damages;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class DamageTestField : Damageable
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time interval to take damages into account in dps measure (seconds).")]
        public float measuringInterval = 2;


        [Header("Parts")]
        public TextMeshPro dpsLabel;
        public TextMeshPro maxDpsLabel;


        // -- Class

        private readonly Queue<DamageEvent> _events = new Queue<DamageEvent>();
        private string _dpsLabelToFormat;
        private string _maxDpsLabelToFormat;

        private float _maxDps;

        void Start()
        {
            _dpsLabelToFormat = dpsLabel.text;
            _maxDpsLabelToFormat = maxDpsLabel.text;
        }

        void Update()
        {
            // calculate dps
            float dps = 0;
            var eventsToKeep = new Queue<DamageEvent>();

            while (_events.Any())
            {
                DamageEvent damageEvent = _events.Dequeue();
                if (damageEvent.Time + measuringInterval < Time.time)
                {
                    // old events
                    continue;
                }

                dps += damageEvent.Amount;
                eventsToKeep.Enqueue(damageEvent);
            }

            dps = dps / measuringInterval;

            while (eventsToKeep.Any())
            {
                _events.Enqueue(eventsToKeep.Dequeue());
            }

            if (_maxDps < dps)
            {
                _maxDps = dps;
            }

            // update labels
            dpsLabel.SetText(string.Format(_dpsLabelToFormat, dps));
            maxDpsLabel.SetText(string.Format(_maxDpsLabelToFormat, _maxDps));
        }

        protected override void OnDamage(VulnerableCollider hitCollider, DamageData damageData, MonoBehaviour damager)
        {
            var damageEvent = new DamageEvent(Time.time, damageData.Amount);
            _events.Enqueue(damageEvent);
        }

        protected override void OnDeath()
        {
            throw new InvalidOperationException($"{gameObject} is supposed to be invulnerable.");
        }

        private class DamageEvent
        {
            public float Time { get; }
            public float Amount { get; }

            public DamageEvent(float time, float amount)
            {
                Time = time;
                Amount = amount;
            }
        }
    }
}