using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerProxy : MonoBehaviour
    {
        private Transform _player;

        void Start()
        {
            var playerHealth = Object.FindObjectOfType<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError($"Unable to find {nameof(PlayerHealth)} in hierarchy. " +
                               $"Tracking the position of the player will not work.");
                return;
            }

            _player = playerHealth.transform;
        }

        void Update()
        {
            if (_player == null)
            {
                return;
            }

            this.transform.position = _player.transform.position;
        }
    }
}