using Assets.Scripts.Weaponry.Projectiles;

namespace Assets.Scripts.Weaponry.Weapons
{
    public class BeamGun : Gun
    {
        public BeamEmitter beamEmitter;

        public override void InitFire()
        {
            beamEmitter.EmitProjectile();
        }

        public override void ReleaseFire()
        {
            // do nothing 
        }
    }
}