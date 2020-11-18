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
            yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.GameplayScene);
        }
    }
}