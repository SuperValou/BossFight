using Assets.Scripts.Damages;
using Assets.Scripts.Huds;
using UnityEngine;

namespace Assets.Scripts.Proxies
{
    public class FoeHealthDisplayProxy : MonoBehaviour
    {
        private FoeHealthDisplay _foeHealthDisplay;

        void Start()
        {
            _foeHealthDisplay = Object.FindObjectOfType<FoeHealthDisplay>();
            if (_foeHealthDisplay == null)
            {
                Debug.LogError($"Unable to find {nameof(FoeHealthDisplay)} in hierarchy. " +
                               $"Health of foes won't be displayed on screen.");
            }
        }

        public void Show(Damageable damageable)
        {
            _foeHealthDisplay?.Show(damageable);
        }
    }
}