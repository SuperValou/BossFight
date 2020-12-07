using TMPro;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class BasicSwitch : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        public string enabledText = "On";
        public string disabledText = "Off";

        public Color enabledColor = Color.green;
        public Color disabledColor = Color.red;

        [Header("Parts")]
        public ActivationSwitch activationSwitch;
        public TextMeshPro label;
        public MeshRenderer colorRenderer;

        // -- Class
        
        void Start()
        {
            Update();
        }

        void Update()
        {
            if (activationSwitch.IsTurnedOn)
            {
                label.SetText(enabledText);
                colorRenderer.material.color = enabledColor;
            }
            else
            {
                label.SetText(disabledText);
                colorRenderer.material.color = disabledColor;
            }
        }
    }
}