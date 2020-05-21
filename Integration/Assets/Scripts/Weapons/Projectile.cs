using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Projectile : MonoBehaviour
    {
        //[Tooltip("Time in second")]
        //public float lifetime = 1;

        //[Tooltip("Speed in m/s")]
        //public float speed = 1;

        //public float damage = 5;

        //public ProjectileImpactScript projectileImpactScriptPrefab;
        
        //// ---
        //private Rigidbody _rigidbody;
        
        //void Start()
        //{
        //    _rigidbody = this.GetOrThrow<Rigidbody>();
        //    _rigidbody.AddForce(_rigidbody.transform.forward * speed, ForceMode.Impulse);
            
        //    Invoke(nameof(DieOut), lifetime);
        //}
        
        //void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.collider.tag == "Player")
        //    {
        //        return;
        //    }

        //    var impactInfo = collision.GetContact(0);
        //    var impactPoint = impactInfo.point + impactInfo.normal * 0.001f; // avoid clipping
        //    var impactOrientation = Quaternion.LookRotation(-1f * impactInfo.normal);
        //    var impactScript = Instantiate(projectileImpactScriptPrefab, impactPoint, impactOrientation);

        //    var foe = collision.gameObject.GetComponent<Foe>();
        //    if (foe != null)
        //    {
        //        foe.TakeDamage(damage);

        //        impactScript.Explode();
        //    }
        //    else
        //    {
        //        impactScript.Impact();
        //    }

            
        //    Destroy(gameObject);
        //}

        //void DieOut()
        //{
        //    var impactScript = Instantiate(projectileImpactScriptPrefab, this.transform.position, this.transform.rotation);
        //    impactScript.DieOut();

        //    if (gameObject)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}