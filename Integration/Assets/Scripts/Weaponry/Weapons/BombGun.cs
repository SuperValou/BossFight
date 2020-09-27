using System;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class BombGun : Gun
    {
        public override void InitFire()
        {
            projectileEmitter.EmitProjectile();
            //AudioSource.PlayOneShot(_shotSound);
        }

        public override void ReleaseFire()
        {
            
        }
    }
}