using Assets.Scripts.Players;

namespace Assets.Scripts.Environments.PowerUps
{
    public class MovementAbilityPowerUp : PowerUp
    {
        public bool grantJump;
        public bool grantDash;
        public bool grantBooster;

        protected override void ApplyPowerUp(Player player)
        {
            if (grantJump)
            {
                player.FirstPersonController.hasJumpAbility = true;
            }

            if (grantDash)
            {
                player.FirstPersonController.hasDashAbility = true;
            }

            if (grantBooster)
            {
                player.FirstPersonController.hasBoosterAbility = true;
            }
        }
    }
}