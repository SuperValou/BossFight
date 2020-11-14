using UnityEngine;

namespace Assets.Scripts.Players
{
    public class AimAssist : MonoBehaviour
    {
        // -- Editor

        public Camera eye;
        public Transform arm;

        public float maxDistance = 100;

        public LayerMask applicableLayers;

        // -- Class

        private readonly Vector3 _rayVector = new Vector3(0.5f, 0.5f, 0);

        void Update()
        {
            Ray ray = eye.ViewportPointToRay(_rayVector);
            
            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, applicableLayers))
            {
                arm.transform.LookAt(raycastHit.point);
            }
            else
            {
                arm.transform.LookAt(arm.transform.position + eye.transform.forward);
            }

            //Debug.DrawRay(eye.transform.position, maxDistance * eye.transform.forward, Color.blue);
            //Debug.DrawRay(arm.transform.position, maxDistance * arm.transform.forward, Color.red);
        }
    }
}