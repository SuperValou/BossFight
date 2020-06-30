using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Projectile : MonoBehaviour, IDamager
    {
        // -- Editor

        [Tooltip("Base damage inflicted by the projectile")]
        public float baseDamage = 1;

        [Tooltip("Max lifetime of the projectile in second")]
        public float maxLifetime = 10;

        [Tooltip("Forward speed in m/s")]
        public float speed = 30;

        [Tooltip("GameObject to spawn if the projectile hit some hard surface")]
        public Impact impactPrefab;

        // -- Class

        protected Rigidbody Rigidbody { get; private set; }

        public float BaseDamage { get; private set; }

        protected virtual void Start()
        {
            BaseDamage = baseDamage;

            Rigidbody = this.GetOrThrow<Rigidbody>();
            Rigidbody.AddForce(Rigidbody.transform.forward * speed, ForceMode.Impulse);

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

        protected abstract void DieOut();
    }
}