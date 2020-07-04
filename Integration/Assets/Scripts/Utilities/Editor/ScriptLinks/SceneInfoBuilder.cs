using System.Collections.Generic;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks
{
    public static class SceneInfoBuilder
    {
        public static SceneInfo Build(Scene scene)
        {
            var sceneInfo = new SceneInfo
            {
                ScenePath = scene.path,
                RootGameObjectInfos = new List<GameObjectInfo>()
            };

            var rootGameObjects = scene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                var objectInfo = Build(rootGameObject);
                sceneInfo.RootGameObjectInfos.Add(objectInfo);
            }
            
            return sceneInfo;
        }

        private static GameObjectInfo Build(GameObject gameObject)
        {
            GameObjectInfo objectInfo = new GameObjectInfo()
            {
                Name = gameObject.name,
                Scripts = new List<ScriptInfo>(),
                Children = new List<GameObjectInfo>()
            };

            var components = gameObject.GetComponents<Component>();
            
            foreach (var component in components)
            {
                if (component == null)
                {
                    var scriptInfo = new ScriptInfo() {Name = "<MISSING SCRIPT>"};
                    objectInfo.Scripts.Add(scriptInfo);
                }

                else if (component is MonoBehaviour script)
                {
                    //var scriptType = script.GetType();
                    //if (scriptType.Assembly.FullName.StartsWith("UnityEngine"))
                    //{
                    //    continue;
                    //}

                    var scriptInfo = Build(script);
                    objectInfo.Scripts.Add(scriptInfo);
                }
            }
            
            foreach (Transform child in gameObject.transform)
            {
                var childInfo = Build(child.gameObject);
                objectInfo.Children.Add(childInfo);
            }
            
            return objectInfo;
        }

        private static ScriptInfo Build(MonoBehaviour monoBehaviour)
        {
            var scriptInfo = new ScriptInfo() {Name = monoBehaviour.GetType().Name};
            return scriptInfo;
        }
    }
}