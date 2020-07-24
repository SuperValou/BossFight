using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.Menus
{
    public abstract class Menu : MonoBehaviour
    {
        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        protected bool IsLoading { get; private set; }

        public void LoadMainScene(SceneId sceneId)
        {
            StartCoroutine(LoadMainSceneAsync(sceneId));
        }

        private IEnumerator LoadMainSceneAsync(SceneId sceneId)
        {
            if (IsLoading)
            {
                yield break;
            }

            IsLoading = true;
            yield return sceneLoadingManager.PreloadMainSceneAsync(sceneId);

            IsLoading = false;
            sceneLoadingManager.Activate(sceneId);
        }
    }
}