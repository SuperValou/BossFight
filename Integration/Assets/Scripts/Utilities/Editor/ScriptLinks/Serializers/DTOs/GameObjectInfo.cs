using System;
using System.Collections.Generic;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs
{
    [Serializable]
    public class GameObjectInfo
    {
        public string Name;
        public List<ScriptInfo> Scripts;
        public List<GameObjectInfo> Children;
    }
}