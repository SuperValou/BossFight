using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerProxy : MonoBehaviour
    {
        private readonly Vector3 _playerCenterOfMassOffset = new Vector3(0, 1.20f, 0);
        private Player _player;
        
        void Awake()
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

            this.transform.position = _player.transform.position + _playerCenterOfMassOffset;
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