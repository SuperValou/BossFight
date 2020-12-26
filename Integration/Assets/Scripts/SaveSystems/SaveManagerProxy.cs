using UnityEngine;

namespace Assets.Scripts.SaveSystems
{
    public class SaveManagerProxy : MonoBehaviour
    {
        private SaveManager _saveManager;

        void Awake()
        {
            _saveManager = GameObject.FindObjectOfType<SaveManager>();
            if (_saveManager == null)
            {
                Debug.LogError($"Unable to find {nameof(SaveManager)} in hierarchy. {nameof(SaveStation)}s won't work.");
            }
        }

        public void RequestToSaveGame()
        {
            _saveManager?.RequestToSaveGame();
        }
    }
}