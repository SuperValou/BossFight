using System;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public class BombGun : Gun
    {
        // -- Editor 

        public ProjectileEmitter bombEmitter;

        // -- Class

        private AudioSource _audioSource;

        protected virtual void Start()
        {
            if (bombEmitter == null)
            {
                throw new ArgumentException($"{DisplayName} ({name}) has a null {nameof(bombEmitter)}: you won't be able to shoot. ");
            }

            _audioSource = this.GetOrThrow<AudioSource>();
        }
        
        public override void InitFire()
        {
            bombEmitter.EmitProjectile();
            //AudioSource.PlayOneShot(_shotSound);
        }

        public override void ReleaseFire()
        {
            
        }
    }
}