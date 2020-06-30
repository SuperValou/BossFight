namespace Assets.Scripts.Weaponry.Weapons
{
    public class BeamGun : ProjectileWeapon
    {
        public override void InitFire()
        {
            ShootProjectile();
        }

        public override void ReleaseFire()
        {
            // do nothing 
        }
    }
}