using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weapons.Projectiles
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        [Tooltip("Time in second")]
        public float lifetime = 1;

        [Tooltip("Speed in m/s")]
        public float speed = 1;
        
        public float baseDamage = 5;

        public ProjectileImpactAnimationScript projectileImpactAnimationScriptPrefab;

        // ---
        private Rigidbody _rigidbody;

        void Start()
        {
            _rigidbody = this.GetOrThrow<Rigidbody>();
            _rigidbody.AddForce(_rigidbody.transform.forward * speed, ForceMode.Impulse);

            Invoke(nameof(DieOut), lifetime);
        }

        void OnCollisionEnter(Collision collision)
        {
            ContactPoint contactData = collision.GetContact(0);

            Vector3 contactPoint = contactData.point + contactData.normal * 0.001f; // avoid clipping
            Quaternion contactOrientation = Quaternion.LookRotation(-1f * contactData.normal);

            HandleContact(contactPoint, contactOrientation, collision.gameObject);
        }

        protected abstract void HandleContact(Vector3 contactPoint, Quaternion contactOrientation, GameObject collidedGameObject);


        void DieOut()
        {
            if (!gameObject)
            {
                return;
            }

            var impactScript = Instantiate(projectileImpactAnimationScriptPrefab, this.transform.position, this.transform.rotation);
            impactScript.DieOut();

            Destroy(gameObject);
        }
    }
}