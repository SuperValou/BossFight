using Assets.Scripts.Damages;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    public class PowerBullet : Projectile
    {
        protected override void HandleContact(Vector3 contactPoint, Quaternion contactOrientation, GameObject collidedGameObject)
        {
            Damageable damageable = collidedGameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damager: this);
            }

            if (impactPrefab != null)
            {
                Instantiate(impactPrefab, contactPoint, contactOrientation);
            }

            Destroy(gameObject);
        }

        protected override void DieOut()
        {
            if (!gameObject)
            {
                return;
            }

            if (impactPrefab != null)
            {
                Instantiate(impactPrefab, this.transform.position, this.transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}