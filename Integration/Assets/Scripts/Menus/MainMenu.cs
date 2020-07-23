using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        // -- Editor

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        private bool _isLoading;

        public void Load()
        {
            if (_isLoading)
            {
                return;
            }

            StartCoroutine(sceneLoadingManager.LoadMainSceneAsync(SceneId.MasterScene));
            _isLoading = true;
        }
    }
}