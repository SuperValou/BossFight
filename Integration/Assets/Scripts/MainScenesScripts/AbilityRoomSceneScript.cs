using System.Collections;
using Assets.Scripts.Huds;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.Players;
using Assets.Scripts.SaveSystems;
using UnityEngine;

namespace Assets.Scripts.MainScenesScripts
{
    public class AbilityRoomSceneScript : MonoBehaviour
    {
        // -- Editor

        [Header("Main managers")]
        public SceneLoadingManager sceneLoadingManager;

        [Header("Room managers")]
        public GameObject roomManagers;


        // -- Class

        void Awake()
        {
            if (roomManagers.activeInHierarchy)
            {
                Debug.LogWarning($"This is a Test Room, but {nameof(roomManagers)} are active in hierarchy. " +
                                 $"This may lead to unmanaged Awake() call order, leading to errors in the console.");
            }
        }

        IEnumerator Start()
        {
            if (!sceneLoadingManager.IsLoaded(SceneId.GameplayScene))
            {
                yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.GameplayScene);
            }

            // enable room managers to simulate room load
            roomManagers.SetActive(true);
        }
    }
}