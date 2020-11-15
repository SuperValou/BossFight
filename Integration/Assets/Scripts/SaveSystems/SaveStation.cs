using UnityEngine;

namespace Assets.Scripts.SaveSystems
{
    public class SaveStation : MonoBehaviour
    {
        // -- Editor

        public string triggeringTag = "Player";
        
        // -- Class
        
        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            // save
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