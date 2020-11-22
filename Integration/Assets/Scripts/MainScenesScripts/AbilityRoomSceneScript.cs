using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.MainScenesScripts
{
    public class AbilityRoomSceneScript : MonoBehaviour
    {
        // -- Editor

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        IEnumerator Start()
        {
            if (!sceneLoadingManager.IsLoaded(SceneId.GameplayScene))
            {
                yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.GameplayScene);
            }
        }
    }
}