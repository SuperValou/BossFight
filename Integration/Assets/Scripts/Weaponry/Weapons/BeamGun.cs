using Assets.Scripts.Weaponry.Projectiles;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class BeamGun : Gun
    {
        public override void InitFire()
        {
            projectileEmitter.EmitProjectile();
        }

        public override void ReleaseFire()
        {
            // do nothing 
        }
    }
}