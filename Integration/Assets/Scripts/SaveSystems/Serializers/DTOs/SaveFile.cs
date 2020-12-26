using System;

namespace Assets.Scripts.SaveSystems.Serializers.DTOs
{
    [Serializable]
    public class SaveFile
    {
        public int serializationVersion;

        public SaveData data;
    }
}