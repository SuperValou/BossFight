using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public class SceneLoadingManagerProxy : MonoBehaviour
    {
        private SceneLoadingManager _sceneLoadingManager;

        void Awake()
        {
            _sceneLoadingManager = GameObject.FindObjectOfType<SceneLoadingManager>();
            if (_sceneLoadingManager == null)
            {
                Debug.LogError($"Unable to find {nameof(SceneLoadingManager)} in hierarchy. Scene loadings won't work.");
            }
        }

        public IEnumerator LoadMainSceneAsync(SceneId sceneId)
        {
            if (_sceneLoadingManager == null)
            {
                yield break;
            }

            yield return _sceneLoadingManager.LoadMainSceneAsync(sceneId);
        }
    }
}