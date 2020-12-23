using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class SpreadGun : Gun
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time between consecutive shots (seconds).")]
        public float recoveryTime = 1;

        // -- Class

        private bool _holdingTrigger;
        private float _lastShotTime;

        public override void InitFire()
        {
            _holdingTrigger = true;
            ShootIfPossible();
        }

        void Update()
        {
            if (!_holdingTrigger)
            {
                return;
            }

            ShootIfPossible();
        }

        public override void ReleaseFire()
        {
            _holdingTrigger = false;
        }

        private void ShootIfPossible()
        {
            if (Time.time < _lastShotTime + recoveryTime)
            {
                return;
            }

            ShootPellets();
            _lastShotTime = Time.time;
        }

        private void ShootPellets()
        {
            projectileEmitter.EmitProjectile();
        }
    }
}