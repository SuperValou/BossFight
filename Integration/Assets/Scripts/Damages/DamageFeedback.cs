using System.Linq;
using Assets.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Damages
{
    public class DamageFeedback : MonoBehaviour
    {
        // -- Editor
        
        public MeshRenderer[] meshRenderers;

        [Tooltip("Global color of the mesh when taking damage.")]
        public Color blinkColor = Color.yellow;

        [Tooltip("Global color of the mesh when taking critical damages.")]
        public Color blinkColorCritical = Color.red;

        [Tooltip("Duration of the damage effect (seconds).")]
        public float highlightDuration = 0.1f;

        // -- Class

        public void Blink()
        {
            Blink(blinkColor);
        }

        public void BlinkCritical()
        {
            Blink(blinkColorCritical);
        }

        private void Blink(Color highlightColor)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                var materialColor = meshRenderer.material.color;

                var tweener = meshRenderer.material.DOColor(endValue: highlightColor, highlightDuration);
                tweener.OnComplete(() => meshRenderer.material.DOColor(endValue: materialColor, highlightDuration));
            }
        }
    }
}