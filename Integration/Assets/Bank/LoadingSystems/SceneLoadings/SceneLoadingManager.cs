using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public class SceneLoadingManager : MonoBehaviour
    {
        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();

        void Awake()
        {
            _sceneLoadingSystem.Initialize();
        }

        public IEnumerator LoadSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.Load(sceneId);
            
            while (!_sceneLoadingSystem.IsLoaded(sceneId))
            {
                yield return null;
            }
        }

        public bool IsLoaded(SceneId sceneId)
        {
            return _sceneLoadingSystem.IsLoaded(sceneId);
        }

        public bool IsLoading(SceneId sceneId, out float progress)
        {
            return _sceneLoadingSystem.IsLoading(sceneId, out progress);
        }

        public void Unload(SceneId sceneId)
        {
            _sceneLoadingSystem.Unload(sceneId);
        }
    }
}