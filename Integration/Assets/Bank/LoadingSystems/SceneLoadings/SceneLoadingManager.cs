using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerator LoadMainSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadSingle(sceneId);

            while (!_sceneLoadingSystem.IsLoaded(sceneId))
            {
                yield return null;
            }

            Debug.Log("C'EST BON WESH");
        }

        public IEnumerator LoadSubSenesAsync(SceneId sceneId)
        {
            return LoadSubSenesAsync(new[] {sceneId});
        }

        public IEnumerator LoadSubSenesAsync(ICollection<SceneId> sceneIds)
        {
            foreach (var sceneId in sceneIds)
            {
                _sceneLoadingSystem.LoadAdditive(sceneId);
            }

            bool done = false;
            while (!done)
            {
                yield return null;
                done = sceneIds.All(si => _sceneLoadingSystem.IsLoaded(si));
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