using System;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Gun : AbstractWeapon
    {
        // -- Editor 

        public ProjectileEmitter projectileEmitter;

        // -- Class

        protected AudioSource AudioSource { get; private set; }

        void Start()
        {
            if (projectileEmitter == null)
            {
                throw new ArgumentException($"{DisplayName} ({name}) has a null {nameof(projectileEmitter)}: you won't be able to shoot. ");
            }

            AudioSource = this.GetOrThrow<AudioSource>();
        }
    }
}