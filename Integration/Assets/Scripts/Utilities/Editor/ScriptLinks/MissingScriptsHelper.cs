using System;
using System.IO;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks
{
    public class MissingScriptsHelper : MonoBehaviour
    {
        private const string SclFileExtension = ".scl";
        
        [MenuItem("Tools/" + nameof(SnapshotSceneLinks))]
        static void SnapshotSceneLinks()
        {
            Scene scene = SceneManager.GetActiveScene();
            var sceneInfo = SceneInfoBuilder.Build(scene);

            string sclFileRelativePath = Path.ChangeExtension(scene.path, SclFileExtension);
            string projectFolder = Application.dataPath.Remove(Application.dataPath.Length - "/Assets".Length);
            string sclFileAbsolutePath = Path.Combine(projectFolder, sclFileRelativePath);

            SceneInfoSerializer serializer = new SceneInfoSerializer();
            serializer.WriteToFile(sceneInfo, sclFileAbsolutePath);

            Debug.Log("Snapshot available at " + sclFileRelativePath);
        }
    }
}
