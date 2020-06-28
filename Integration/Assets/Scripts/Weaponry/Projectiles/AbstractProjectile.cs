using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class AbstractProjectile : MonoBehaviour, IDamager
    {
        public float baseDamage = 1;

        [Tooltip("Time in second")]
        public float maxLifetime = 1;

        [Tooltip("Speed in m/s")]
        public float speed = 10;
        
        public ProjectileImpactAnimationScript projectileImpactAnimationScriptPrefab;

        // ---
        private Rigidbody _rigidbody;

        public float BaseDamage { get; private set; }

        void Start()
        {
            BaseDamage = baseDamage;

            _rigidbody = this.GetOrThrow<Rigidbody>();
            _rigidbody.AddForce(_rigidbody.transform.forward * speed, ForceMode.Impulse);

            Invoke(nameof(DieOut), maxLifetime);
        }

        void OnCollisionEnter(Collision collision)
        {
            ContactPoint contactData = collision.GetContact(0);

            Vector3 contactPoint = contactData.point + contactData.normal * 0.001f; // avoid clipping
            Quaternion contactOrientation = Quaternion.LookRotation(-1f * contactData.normal);

            HandleContact(contactPoint, contactOrientation, collision.gameObject);
        }

        protected abstract void HandleContact(Vector3 contactPoint, Quaternion contactOrientation, GameObject collidedGameObject);
        
        private void DieOut()
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