using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerProxy : MonoBehaviour
    {
        private Player _player;

        void Start()
        {
            _player = Object.FindObjectOfType<Player>();
            if (_player == null)
            {
                Debug.LogError($"Unable to find {nameof(Player)} in hierarchy. " +
                               $"Tracking the position of the player will not work.");
            }
        }

        void Update()
        {
            if (_player == null)
            {
                return;
            }

            this.transform.position = _player.transform.position;
        }

        public void SetInvulnerability(bool isInvulnerable)
        {
            if (_player == null)
            {
                return;
            }

            _player.isInvulnerable = isInvulnerable;
        }
    }
}