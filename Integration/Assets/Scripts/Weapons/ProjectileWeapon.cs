using System.Collections;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weapons.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public class ProjectileWeapon : AbstractWeapon
    {
        [Header("Self")]

        [Tooltip("Does holding the fire button make the weapon charge?")]
        public bool canCharge = true;

        [Tooltip("Time in second to fully charge")]
        public float timeToCharge = 3;

        [Tooltip("Minimum charge to shoot a charged blast")]
        public float chargeThreshold = 0.5f;

        [Tooltip("How many projectiles in a fully charged shot?")]
        public int chargedProjectileCount = 10;

        [Tooltip("Seconds between each projectile in a fully charged shot")]
        public float timeBetweenChargedShot = 0.1f;
        

        [Header("Parts")]
        public AbstractProjectile projectilePrefab;
        public AbstractProjectile chargedProjectilePrefab;

        [Header("Anims")]
        public GameObject chargeAnimationPrefab;

        [Header("Sounds")]
        public AudioClip _shotSound;
        public AudioClip _chargedShotSound;

        // ---
        private float _holdTime = 0;
        private bool _isCharging = false;
        private bool _isChargeRafaleShooting = false;

        private AudioSource _audioSource;

        private GameObject _chargeAnimationObject;
        
        public float Charge { get; private set; } = 0;

        void Start()
        {
            _audioSource = this.GetOrThrow<AudioSource>();
        }

        public override void Fire()
        {
            if (_isChargeRafaleShooting)
            {
                return;
            }

            ShootProjectile();

            // begin charge
            if (!canCharge)
            {
                return;
            }

            _holdTime = Time.time;
            _isCharging = true;

            _chargeAnimationObject = Instantiate(chargeAnimationPrefab, this.transform.position, this.transform.rotation);
            _chargeAnimationObject.transform.SetParent(this.transform);
        }

        public override void ReleaseFire()
        {
            if (_isChargeRafaleShooting)
            {
                return;
            }

            _isCharging = false;
            Destroy(_chargeAnimationObject);
            _chargeAnimationObject = null;
            
            if (Charge > chargeThreshold)
            {
                _isChargeRafaleShooting = true;
                _audioSource.Stop();
                StartCoroutine(ShootChargedRafale());
            }
            else
            {
                Charge = 0;
            }
        }

        private IEnumerator ShootChargedRafale()
        {
            if (Charge < 1)
            {
                var wait = new WaitForSeconds(timeBetweenChargedShot);
                int projectileCount = (int)(chargedProjectileCount * Charge);
                for (int i = 0; i < projectileCount; i++)
                {
                    ShootProjectile();
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
            
            Charge = 0;
            _isChargeRafaleShooting = false;
        }

        private void ShootProjectile()
        {
            Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
            _audioSource.PlayOneShot(_shotSound);
        }

        private void ShootChargedProjectile()
        {
            Instantiate(chargedProjectilePrefab, this.transform.position, this.transform.rotation);

            _audioSource.PlayOneShot(_chargedShotSound);

            //Sequence s = DOTween.Sequence();
            //s.Append(cannonModel.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity));
            //s.Join(cannonModel.GetComponentInChildren<Renderer>().material.DOColor(normalEmissionColor, "_EmissionColor", punchDuration));
            //s.Join(cannonModel.DOLocalMove(cannonLocalPos, punchDuration).SetDelay(punchDuration));
        }

        void Update()
        {
            if (_isCharging && timeToCharge > 0)
            {
                Charge = Mathf.Clamp01((Time.time - _holdTime) / timeToCharge);
            }
        }
    }
}