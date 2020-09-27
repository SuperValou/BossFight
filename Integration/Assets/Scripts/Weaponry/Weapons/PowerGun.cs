using System.Collections;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class PowerGun : Gun
    {
        // -- Editor
        [Header("Self")]
        [Tooltip("Minimum charge to shoot a charged blast")]
        public float chargeThreshold = 0.5f;

        [Tooltip("How many projectiles in a fully charged shot?")]
        public int chargedProjectileCount = 10;

        [Tooltip("Seconds between each projectile in a fully charged shot")]
        public float timeBetweenChargedShot = 0.1f;


        [Header("Parts")]
        public Projectile chargedProjectilePrefab;

        [Header("Sounds")]
        public AudioClip _chargedShotSound;

        [Header("Anims")]
        public GameObject chargeAnimationPrefab;

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

            //Shoot();

            // begin charge
            _charge.Begin();
            
            _chargeAnimationObject = Instantiate(chargeAnimationPrefab, this.transform.position, this.transform.rotation);
            _chargeAnimationObject.transform.SetParent(this.transform);
        }

        public override void ReleaseFire()
        {
            if (_isChargeRafaleShooting)
            {
                return;
            }

            // end charge
            _charge.Stop();

            Destroy(_chargeAnimationObject);
            _chargeAnimationObject = null;

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
                int projectileCount = (int)(chargedProjectileCount * _charge.Value);
                for (int i = 0; i < projectileCount; i++)
                {
                    //Shoot();
                    yield return wait;
                }
            }
            else
            {
                var wait = new WaitForSeconds(timeBetweenChargedShot / 2f);
                int projectileCout = chargedProjectileCount * 2;
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
            Instantiate(chargedProjectilePrefab, this.transform.position, this.transform.rotation);

            //AudioSource.PlayOneShot(_chargedShotSound);

            //Sequence s = DOTween.Sequence();
            //s.Append(cannonModel.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity));
            //s.Join(cannonModel.GetComponentInChildren<Renderer>().material.DOColor(normalEmissionColor, "_EmissionColor", punchDuration));
            //s.Join(cannonModel.DOLocalMove(cannonLocalPos, punchDuration).SetDelay(punchDuration));
        }
    }
}