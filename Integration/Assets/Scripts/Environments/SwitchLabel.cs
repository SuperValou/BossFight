using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    [RequireComponent(typeof(TextMeshPro))]
    public class SwitchLabel : MonoBehaviour
    {
        // -- Editor

        public ActivationSwitch activationSwitch;
        public string enabledText;
        public string disabledText;

        // -- Class

        private TextMeshPro _label;
        
        void Start()
        {
            _label = this.GetOrThrow<TextMeshPro>();
        }

        void Update()
        {
            _label.SetText(activationSwitch.IsTurnedOn ? enabledText : disabledText);
        }
    }
}