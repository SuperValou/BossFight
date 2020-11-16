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
        }

        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            // save
            _saveManager.SaveGame();
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