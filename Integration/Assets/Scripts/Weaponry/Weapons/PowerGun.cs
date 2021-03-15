using System.Collections;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Charges;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class PowerGun : Gun
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time between partially charged shots (seconds).")]
        public float timeBetweenRafaleShot = 0.2f;

        [Tooltip("Max number of shots fired by a partial charge.")]
        public int maxRafaleShotCount = 3;


        [Header("Parts")]
        public ProjectileEmitter chargedProjectileEmitter;
        

        // -- Class

        private WaitForSeconds _waitBetweenRafaleShot;
        private WeaponCharge _charge;

        private bool _isRafaleShooting;

        protected override void Start()
        {
            base.Start();
            _charge = this.GetOrThrow<WeaponCharge>();
            _waitBetweenRafaleShot = new WaitForSeconds(timeBetweenRafaleShot);
        }

        public override void InitFire()
        {
            _charge.Begin();

            if (_isRafaleShooting)
            {
                return;
            }

            projectileEmitter.EmitProjectile();
        }
        
        public override void ReleaseFire()
        {
            _charge.Stop();
            
            if (_charge.IsFullyCharged)
            {
                ShootChargedProjectile();
                _charge.Clear();
            }
            else if (_charge.IsMinimalyCharged)
            {
                StartCoroutine(ShootRafale());
            }
            else
            {
                _charge.Clear();
            }
        }

        private IEnumerator ShootRafale()
        {
            _isRafaleShooting = true;

            float charge = _charge.ChargeValue + _charge.minChargeThreshold;
            int projCount = Mathf.Max(1, Mathf.FloorToInt(charge * maxRafaleShotCount));
            for (int i = 0; i < projCount; i++)
            {
                projectileEmitter.EmitProjectile();
                yield return _waitBetweenRafaleShot;
            }

            _isRafaleShooting = false;
            _charge.Clear();
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