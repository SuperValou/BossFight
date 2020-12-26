using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Damages
{
    public class DamageFeedback : MonoBehaviour
    {
        // -- Editor
        
        [Tooltip("Global color of the mesh when taking damage.")]
        public Color blinkColor = Color.yellow;

        [Tooltip("Global color of the mesh when taking critical damages.")]
        public Color blinkColorCritical = Color.red;

        [Tooltip("Duration of the damage effect (seconds).")]
        public float blinkDuration = 0.1f;

        // -- Class

        private MeshRenderer[] _meshRenderers;
        private Color[] _originalColors;

        void Start()
        {
            _meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
            _originalColors = new Color[_meshRenderers.Length];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _originalColors[i] = _meshRenderers[i].material.color;
            }
        }

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
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                var meshRenderer = _meshRenderers[i];
                var originalColor = _originalColors[i];
                var tweener = meshRenderer.material.DOColor(endValue: highlightColor, blinkDuration);
                tweener.OnComplete(() => meshRenderer.material.DOColor(endValue: originalColor, blinkDuration));
            }
        }
    }
}