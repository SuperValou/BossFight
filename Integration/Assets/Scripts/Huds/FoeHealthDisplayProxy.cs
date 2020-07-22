using Assets.Scripts.Damages;
using UnityEngine;

namespace Assets.Scripts.Huds
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