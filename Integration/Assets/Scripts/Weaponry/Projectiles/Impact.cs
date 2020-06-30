using System.Linq;
using Assets.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    public class Impact : MonoBehaviour
    {
        // -- Editor

        public float attenuationTime = 3;

        // -- Class

        private MeshRenderer _renderer;

        void Start()
        {
            _renderer = this.GetOrThrow<MeshRenderer>();
            _renderer.materials.First().DOFade(0, attenuationTime);
            Invoke(nameof(AutoDestroy), attenuationTime + 1);
        }

        private void AutoDestroy()
        {
            Destroy(this.gameObject);
        }
    }
}