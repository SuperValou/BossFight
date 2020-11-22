using UnityEngine;

namespace Assets.Scripts.SaveSystems
{
    public class SaveStation : MonoBehaviour
    {
        // -- Editor

        public string triggeringTag = "Player";
        
        // -- Class

        private SaveManager _saveManager;

        void Start()
        {
            _saveManager = GameObject.FindObjectOfType<SaveManager>();
            if (_saveManager == null)
            {
                Debug.LogError($"Unable to find {nameof(SaveManager)} in hierarchy. {nameof(SaveStation)} won't work.");
            }
        }

        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            // save
            _saveManager.RequestToSaveGame();
        }

        void OnTriggerExit(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            // back to normal 
        }
    }
}