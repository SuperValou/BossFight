using Assets.Scripts.Environments;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes
{
    public class TrainingTurret : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time between consecutive shots (in seconds)")]
        public float shotDelay = 2f;

        [Header("Parts")]
        public Transform cannon;
        public ProjectileEmitter projectileEmitter;
        public ActivationSwitch activationActivationSwitch;

        [Header("References")]
        public Transform target;

        // -- Class

        private float _lastShotTime;

        void Start()
        {
            if (target == null)
            {
                Debug.LogWarning($"{this.GetType().Name} ({name}) has a null {nameof(target)}.");
            }
        }

        void Update()
        {
            if (activationActivationSwitch.IsTurnedOff || target == null)
            {
                _lastShotTime = Time.time;
                return;
            }

            cannon.LookAt(target);

            if (Time.time < _lastShotTime + shotDelay)
            {
                return;
            }

            projectileEmitter.EmitProjectile();
        }
    }
}