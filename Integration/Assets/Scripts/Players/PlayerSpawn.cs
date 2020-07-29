using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerSpawn : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Load the gameplay scene?")]
        public SceneId gameplay = SceneId.GameplayScene;

        [Tooltip("First room to spawn")]
        public SceneId initialRoom = SceneId.BossRoomScene;

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        private Player _player;

        IEnumerator Start()
        {
            yield return sceneLoadingManager.LoadSubSenesAsync(new[] { gameplay, initialRoom });

            _player = GameObject.FindObjectOfType<Player>();
            if (_player == null)
            {
                Debug.LogWarning($"{nameof(Player)} was not found in hierarchy. It won't be spawned at '{this.transform.position}'.");
                yield break;
            }

            _player.transform.position = this.transform.position;
            _player.transform.rotation = this.transform.rotation;
        }
    }
}