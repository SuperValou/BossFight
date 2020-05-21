using Assets.Scripts.Foes;
using UnityEngine;

namespace Assets.Scripts.Weapons.Projectiles
{
    public class BulletProjectile : AbstractProjectile
    {
        protected override void HandleContact(Vector3 contactPoint, Quaternion contactOrientation, GameObject collidedGameObject)
        {
            var impactScript = Instantiate(projectileImpactAnimationScriptPrefab, contactPoint, contactOrientation);

            var foe = collidedGameObject.GetComponent<Foe>();
            if (foe != null)
            {
                foe.TakeDamage(baseDamage);

                impactScript.ImpactFoe();
            }
            else
            {
                impactScript.ImpactInertSurface();
            }

            Destroy(gameObject);
        }
    }
}