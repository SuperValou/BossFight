using System.Collections;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class ProjectileWeapon : AbstractWeapon
    {
        // -- Editor 

        public Projectile projectilePrefab;
        
        public AudioClip _shotSound;
        
        // -- Class
        
        protected AudioSource AudioSource;
        
        protected virtual void Start()
        {
            AudioSource = this.GetOrThrow<AudioSource>();
        }

        protected Projectile ShootProjectile()
        {
            Projectile projectile = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
            AudioSource.PlayOneShot(_shotSound);

            return projectile;
        }
    }
}