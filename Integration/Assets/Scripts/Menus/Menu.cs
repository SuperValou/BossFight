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
            if (IsLoading)
            {
                return;
            }

            StartCoroutine(sceneLoadingManager.LoadMainSceneAsync(sceneId));
            IsLoading = true;
        }
    }
}