using System.Collections;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class PowerGun : Gun
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Minimum charge to shoot a charged blast")]
        public float chargeThreshold = 0.2f;

        [Tooltip("How many projectiles in a fully charged shot?")]
        public int chargedProjectileCount = 10;

        [Tooltip("Seconds between each projectile in a fully charged shot")]
        public float timeBetweenChargedShot = 0.1f;


        [Header("Parts")]
        public ProjectileEmitter chargedProjectileEmitter;
        public ParticleSystem chargeEmitter;
        
        // -- Class

        private bool _isChargeRafaleShooting = false;

        private WeaponCharge _charge;

        private GameObject _chargeAnimationObject;

        protected void Start()
        {
            _charge = this.GetOrThrow<WeaponCharge>();
        }

        public override void InitFire()
        {
            if (_isChargeRafaleShooting)
            {
                return;
            }

            projectileEmitter.EmitProjectile();

            // begin charge
            _charge.Begin();
            chargeEmitter.Play();
        }

        public override void ReleaseFire()
        {
            if (_isChargeRafaleShooting)
            {
                return;
            }

            // end charge
            _charge.Stop();
            chargeEmitter.Stop();
            chargeEmitter.Clear();

            if (_charge.Value > chargeThreshold)
            {
                _isChargeRafaleShooting = true;
                //AudioSource.Stop();
                StartCoroutine(ShootChargedRafale());
            }
            else
            {
                _charge.Clear();
            }
        }

        private IEnumerator ShootChargedRafale()
        {
            if (_charge.Value < 1)
            {
                var wait = new WaitForSeconds(timeBetweenChargedShot);
                int projectileCount = (int) (chargedProjectileCount * _charge.Value);
                for (int i = 0; i < projectileCount; i++)
                {
                    projectileEmitter.EmitProjectile();
                    yield return wait;
                }
            }
            else
            {
                var wait = new WaitForSeconds(timeBetweenChargedShot / 2f);
                int projectileCout = chargedProjectileCount;
                for (int j = 0; j < projectileCout; j++)
                {
                    ShootChargedProjectile();
                    yield return wait;
                }
            }

            _charge.Clear();
            _isChargeRafaleShooting = false;
        }

        private void ShootChargedProjectile()
        {
            chargedProjectileEmitter.EmitProjectile();

            //AudioSource.PlayOneShot(_chargedShotSound);

            //Sequence s = DOTween.Sequence();
            //s.Append(cannonModel.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity));
            //s.Join(cannonModel.GetComponentInChildren<Renderer>().material.DOColor(normalEmissionColor, "_EmissionColor", punchDuration));
            //s.Join(cannonModel.DOLocalMove(cannonLocalPos, punchDuration).SetDelay(punchDuration));
        }
    }
}