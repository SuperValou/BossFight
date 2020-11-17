using System;
using System.IO;
using Assets.Scripts.SaveSystems.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.SaveSystems.Serializers
{
    public class SaveFileReader
    {
        private readonly string _saveFilePath;

        public SaveFileReader(string saveFilePath)
        {
            _saveFilePath = saveFilePath ?? throw new ArgumentNullException(nameof(saveFilePath));
        }

        public SaveData Read()
        {
            if (!File.Exists(_saveFilePath))
            {
                throw new FileNotFoundException($"Unable to read save file, because it was not found at \"{_saveFilePath}\".", _saveFilePath);
            }

            string serializedSaveData = File.ReadAllText(_saveFilePath);
            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(serializedSaveData);
            if (saveFile.serializationVersion != SerializationConstants.Version)
            {
                throw new ArgumentException($"Save file is in incorrect version {saveFile.serializationVersion}. " +
                                            $"Only version {SerializationConstants.Version} is handled.");
            }

            return saveFile.data;
        }
    }
}