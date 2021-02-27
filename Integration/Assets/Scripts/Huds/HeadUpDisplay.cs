using Assets.Scripts.Damages;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Huds
{
    public class HeadUpDisplay : MonoBehaviour, IDamageNotifiable
    {
        // -- Editor

        public float shakeDuration = 1f;
        public float shakeStrength = 100f;
        public int shakeFrequency = 30;

        // -- Class
        private RectTransform _rectTransform;
        
        void Start()
        {
            _rectTransform = this.GetOrThrow<RectTransform>();
        }
        
        public void OnDamageNotification(Damageable damageable, DamageData damageData, MonoBehaviour damager)
        {
            // shake 
            // TODO: DOShakeAnchorPos is buggy
            //_rectTransform.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeFrequency);
        }

        public void OnDeathNotification(Damageable damageable)
        {
            // do something maybe
        }
    }
}