using Assets.Scripts.Environments;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Foes
{
    public class TrainingTurret : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Time between consecutive shots (in seconds)")]
        public float shotDelay = 2f;

        [Header("Parts")]
        public Transform cannon;
        public Projectile projectilePrefab;

        [Header("References")]
        public Switch activationSwitch;
        public Transform target;

        // -- Class

        private float _lastShotTime;
        
        void Update()
        {
            if (activationSwitch.IsTurnedOff || target == null)
            {
                _lastShotTime = Time.time;
                return;
            }

            cannon.LookAt(target);

            if (Time.time < _lastShotTime + shotDelay)
            {
                return;
            }
            
            Instantiate(projectilePrefab, cannon.transform.position, cannon.transform.rotation);
            _lastShotTime = Time.time;
        }
    }
}