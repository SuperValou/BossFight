using System;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs
{
    [Serializable]
    public class SceneLinkFile
    {
        public int SerializationMajorVersion;
        public SceneInfo SceneInfo;
    }
}