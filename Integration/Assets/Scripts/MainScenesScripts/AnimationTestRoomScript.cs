using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.MainScenesScripts
{
    public class AnimationTestRoomScript : MonoBehaviour
    {
        // -- Editor

        [Header("Main managers")]
        public SceneLoadingManager sceneLoadingManager;

        [Header("Room objects")]
        public GameObject roomManagers;

        public GameObject[] roomStart;

        // -- Class

        void Awake()
        {
            if (roomManagers.activeInHierarchy)
            {
                Debug.LogWarning($"This is a Test Room, but {nameof(roomManagers)} are active in hierarchy. " +
                                 $"This may lead to unmanaged Awake() call order, leading to errors in the console.");
            }

            foreach (var gameObj in roomStart)
            {
                if (gameObj.activeInHierarchy)
                {
                    Debug.LogWarning($"This is a Test Room, but {gameObj} is active in hierarchy. " +
                                     $"This may lead to unmanaged Start() call order, leading to broken feature.");
                }
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
            foreach (var gameObj in roomStart)
            {
                gameObj.SetActive(true);
            }
        }
    }
}