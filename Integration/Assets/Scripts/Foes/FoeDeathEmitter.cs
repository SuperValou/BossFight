using Assets.Scripts.Damages;
using UnityEngine;

namespace Assets.Scripts.Foes
{
    public class FoeDeathEmitter : MonoBehaviour, IDamageNotifiable
    {
        // -- Editor
        
        // -- Class

        private ParticleSystem _emitter;

        void Start()
        {
            _emitter = this.GetComponent<ParticleSystem>();
        }

        public void OnDamageNotification(Damageable damageable, DamageData damageData, MonoBehaviour damager)
        {
            // do nothing
        }

        public void OnDeathNotification(Damageable damageable)
        {
            if (damageable == null)
            {
                return;
            }

            _emitter.transform.position = damageable.transform.position;
            _emitter.transform.rotation = Quaternion.LookRotation(damageable.transform.up);
            _emitter.Emit(30);
        }
    }
}