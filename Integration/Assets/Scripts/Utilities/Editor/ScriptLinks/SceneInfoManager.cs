using System;
using System.IO;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks
{
    public class SceneInfoManager : ISceneInfoManager
    {
        private const string SclFileExtension = ".scl";

        private SceneInfo _sceneInfo;

        public bool IsInitialized => _sceneInfo != null;

        public void InitializeWithCurrentScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            _sceneInfo = SceneInfoBuilder.Build(scene);
        }

        public void SnapshotSceneInfo()
        {
            if (_sceneInfo == null)
            {
                throw new InvalidOperationException($"Nothing to snapshot yet. Call {nameof(InitializeWithCurrentScene)} before.");
            }

            string sclFileRelativePath = Path.ChangeExtension(_sceneInfo.ScenePath, SclFileExtension);
            string projectFolder = Application.dataPath.Remove(Application.dataPath.Length - "/Assets".Length);
            string sclFileAbsolutePath = Path.Combine(projectFolder, sclFileRelativePath);

            SceneInfoSerializer serializer = new SceneInfoSerializer();
            serializer.WriteToFile(_sceneInfo, sclFileAbsolutePath);

            Debug.Log($"Snapshot available at '{sclFileAbsolutePath}'.");
        }
    }
}
