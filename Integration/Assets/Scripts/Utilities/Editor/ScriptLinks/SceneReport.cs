using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks
{
    public class SceneReport
    {
        private const string MissingTag = "<MISSING>";

        private readonly Scene _scene;

        public string SceneName { get; }

        public SceneInfo SceneInfo { get; private set; }

        public ICollection<string> ScriptNames { get; } = new List<string>();
        public ICollection<string> MissingScriptPaths { get; } = new List<string>();
        
        public SceneReport(Scene scene)
        {
            _scene = scene;
            SceneName = scene.name;
        }

        public void Build()
        {
            var sceneInfo = new SceneInfo
            {
                SceneName = _scene.name,
                ScenePath = _scene.path,
                RootGameObjectInfos = new List<GameObjectInfo>()
            };

            IEnumerable<GameObject> rootGameObjects = _scene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                GameObjectInfo objectInfo = Build(rootGameObject, hierarchyPath: string.Empty);
                sceneInfo.RootGameObjectInfos.Add(objectInfo);
            }
            
            SceneInfo = sceneInfo;
        }

        private GameObjectInfo Build(GameObject gameObject, string hierarchyPath)
        {
            GameObjectInfo objectInfo = new GameObjectInfo()
            {
                Name = gameObject.name,
                Scripts = new List<ScriptInfo>(),
                Children = new List<GameObjectInfo>()
            };

            var components = gameObject.GetComponents<Component>();
            
            for (int i = 0; i < components.Length; i++)
            {
                Component component = components[i];

                if (component == null)
                {
                    var missingScriptInfo = new ScriptInfo()
                    {
                        Name = MissingTag,
                        FullName = MissingTag,
                        Index = i
                    };

                    objectInfo.Scripts.Add(missingScriptInfo);

                    string missingScriptPath = Path.Combine(hierarchyPath, gameObject.name, i.ToString());
                    this.MissingScriptPaths.Add(missingScriptPath);
                }

                else if (component is MonoBehaviour script)
                {
                    var scriptInfo = new ScriptInfo()
                    {
                        Name = script.GetType().Name,
                        FullName = script.GetType().FullName,
                        Index = i
                    };

                    objectInfo.Scripts.Add(scriptInfo);

                    ScriptNames.Add(scriptInfo.Name);
                }
            }

            string parentHierarchy = Path.Combine(hierarchyPath, gameObject.name);
            foreach (Transform child in gameObject.transform)
            {
                // if this child is the empty instance, we skip it
                GameObjectInfo childInfo = Build(child.gameObject, hierarchyPath: parentHierarchy);
                if (childInfo == GameObjectInfo.Empty)
                {
                    continue;
                }

                objectInfo.Children.Add(childInfo);
            }

            // if this gameobject has no script and no children, we don't care about it and return the empty instance
            if (objectInfo.Scripts.Count == 0 && objectInfo.Children.Count == 0)
            {
                return GameObjectInfo.Empty;
            }

            return objectInfo;
        }
    }
}