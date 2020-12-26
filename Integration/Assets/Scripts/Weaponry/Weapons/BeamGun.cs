using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Charges;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class BeamGun : Gun
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time between consecutive shots (seconds).")]
        public float recoveryTime = 0.1f;

        [Header("Parts")]
        public ProjectileEmitter mediumBeamEmitter;
        public ProjectileEmitter bigBeamEmitter;

        // -- Class

        private float _lastShotTime;
        private WeaponCharge _charge;

        protected override void Start()
        {
            base.Start();
            _charge = this.GetOrThrow<WeaponCharge>();
        }

        public override void InitFire()
        {
            if (Time.time > _lastShotTime + recoveryTime)
            {
                ShootSmallBeam();
                _lastShotTime = Time.time;
            }
            
            _charge.Begin();
        }

        public override void ReleaseFire()
        {
            _charge.Stop();

            if (!_charge.IsMinimalyCharged)
            {
                _charge.Clear();
                return;
            }

            if (_charge.IsFullyCharged)
            {
                ShootBigBeam();
            }
            else
            {
                ShootMediumBeam();
            }

            _lastShotTime = Time.time;
            _charge.Clear();
        }

        private void ShootSmallBeam()
        {
            projectileEmitter.EmitProjectile();
        }

        private void ShootMediumBeam()
        {
            mediumBeamEmitter.EmitProjectile();
        }

        private void ShootBigBeam()
        {
            bigBeamEmitter.EmitProjectile();
        }
    }
}