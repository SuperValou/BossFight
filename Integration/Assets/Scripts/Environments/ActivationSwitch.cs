using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class ActivationSwitch : MonoBehaviour
    {
        // -- Editor

        public bool initialState = false;
        
        // -- Class

        public bool IsTurnedOn { get; private set; }
        public bool IsTurnedOff => !IsTurnedOn;
        
        void Awake()
        {
            IsTurnedOn = initialState;
        }

        public void Flip()
        {
            IsTurnedOn = !IsTurnedOn;
        }
    }
}