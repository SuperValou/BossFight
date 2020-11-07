using System;
using System.Collections.Generic;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs
{
    [Serializable]
    public class SceneInfo
    {
        public string SceneName;
        public string ScenePath;
        public List<GameObjectInfo> RootGameObjectInfos;
    }
}