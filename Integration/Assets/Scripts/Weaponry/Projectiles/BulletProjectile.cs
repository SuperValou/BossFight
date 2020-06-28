using Assets.Scripts.Damages;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
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