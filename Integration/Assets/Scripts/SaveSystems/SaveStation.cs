using UnityEngine;

namespace Assets.Scripts.SaveSystems
{
    public class SaveStation : MonoBehaviour
    {
        // -- Editor

        public string triggeringTag = "Player";

        public SaveManagerProxy saveManagerProxy;

        // -- Class
        
        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            // save
            saveManagerProxy.RequestToSaveGame();
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