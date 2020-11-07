using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class ActivationSwitch : MonoBehaviour
    {
        // -- Editor

        public bool turnedOnOnStart = false;
        
        // -- Class

        public bool IsTurnedOn { get; private set; }
        public bool IsTurnedOff => !IsTurnedOn;
        
        void Start()
        {
            IsTurnedOn = turnedOnOnStart;
        }

        public void Flip()
        {
            IsTurnedOn = !IsTurnedOn;
        }
    }
}