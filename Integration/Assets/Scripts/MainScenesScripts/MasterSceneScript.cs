using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.MainScenesScripts
{
    public class MasterSceneScript : MonoBehaviour
    {
        // -- Editor

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        IEnumerator Start()
        {
            yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.GameplayScene);

            var player = GameObject.FindObjectOfType<Player>();
            player.gameObject.SetActive(false);

            yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.FirstRoomScene);

            player.gameObject.SetActive(true);
        }
    }
}