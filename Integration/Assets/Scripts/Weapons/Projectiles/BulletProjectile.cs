using Assets.Scripts.Damages;
using Assets.Scripts.Foes;
using UnityEngine;

namespace Assets.Scripts.Weapons.Projectiles
{
    public class BulletProjectile : AbstractProjectile
    {
        protected override void HandleContact(Vector3 contactPoint, Quaternion contactOrientation, GameObject collidedGameObject)
        {
            var impactScript = Instantiate(projectileImpactAnimationScriptPrefab, contactPoint, contactOrientation);

            var damageable = collidedGameObject.GetComponent<Damageable>();
            
            if (damageable != null)
            {
                damageable.TakeDamage(damager: this);
                impactScript.ImpactDamageable();
            }
            else
            {
                impactScript.ImpactInertSurface();
            }

            Destroy(gameObject);
        }
    }
}