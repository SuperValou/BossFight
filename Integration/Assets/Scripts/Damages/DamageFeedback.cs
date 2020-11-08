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
        [ColorUsage(showAlpha: true, hdr: true)]
        public Color highlightColor;

        [Tooltip("Duration of the damage effect (seconds).")]
        public float highlightDuration = 0.1f;

        // -- Class

        public void Blink()
        {
            //foreach (var meshRenderer in meshRenderers)
            //{
            //    var materialColor = meshRenderer.material.color;

            //    var tweener = meshRenderer.material.DOColor(endValue: highlightColor, highlightDuration);
            //    tweener.OnComplete(() => meshRenderer.material.DOColor(endValue: materialColor, highlightDuration));
            //}
        }
    }
}