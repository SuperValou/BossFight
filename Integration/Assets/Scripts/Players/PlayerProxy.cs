using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerProxy : MonoBehaviour
    {
        private Transform _player;

        void Start()
        {
            var player = Object.FindObjectOfType<Player>();
            if (player == null)
            {
                Debug.LogError($"Unable to find {nameof(Player)} in hierarchy. " +
                               $"Tracking the position of the player will not work.");
                return;
            }

            _player = player.transform;
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