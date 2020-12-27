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