namespace Assets.Scripts.Weaponry.Weapons
{
    public class SpreadGun : Gun
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