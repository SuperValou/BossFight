using System;
using System.IO;
using Assets.Scripts.SaveSystems.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.SaveSystems.Serializers
{
    public class SaveFileWriter
    {
        private readonly string _filePath;

        public SaveFileWriter(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Overwrite(SaveData saveData)
        {
            SaveFile saveFile = new SaveFile()
            {
                serializationVersion = SerializationConstants.Version,
                data = saveData
            };

            string serializedSaveData = JsonUtility.ToJson(saveFile);
            File.WriteAllText(_filePath, serializedSaveData);
        }
    }
}