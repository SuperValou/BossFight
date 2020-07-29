using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerSpawn : MonoBehaviour
    {
        private Player _player;

        void Start()
        {
            _player = GameObject.FindObjectOfType<Player>();
            if (_player == null)
            {
                Debug.LogWarning($"{nameof(Player)} was not found in hierarchy. It won't be spawned at '{this.transform.position}'.");
                return;
            }

            _player.transform.position = this.transform.position;
            _player.transform.rotation = this.transform.rotation;
        }
    }
}