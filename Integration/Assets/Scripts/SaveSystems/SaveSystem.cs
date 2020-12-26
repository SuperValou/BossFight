using System;
using System.IO;
using Assets.Scripts.SaveSystems.Serializers;
using Assets.Scripts.SaveSystems.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.SaveSystems
{
    public class SaveSystem : ISaveSystem
    {
        public int MaxSlotsCount => 10;
        public string SaveFolder { get; private set; }

        public void Initialize()
        {
            string persitentFolderPath = Application.persistentDataPath;
            string saveFolder = Path.Combine(persitentFolderPath, "Saves");
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            SaveFolder = saveFolder;
        }
        
        public bool IsFree(int slotIndex)
        {
            if (SaveFolder == null)
            {
                throw new InvalidOperationException($"{nameof(SaveSystem)} is not initilized. Did you forget to call the {nameof(Initialize)} method?");
            }

            if (slotIndex < 0 || slotIndex > MaxSlotsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(slotIndex), $"{nameof(slotIndex)} cannot be less than 0 or greater than {MaxSlotsCount} (was {slotIndex}).");
            }

            string filepath = GetSaveFilePath(slotIndex);
            bool saveFileExists = File.Exists(filepath);
            return !saveFileExists;
        }

        public SaveData LoadData(int slotIndex)
        {
            if (SaveFolder == null)
            {
                throw new InvalidOperationException($"{nameof(SaveSystem)} is not initilized. Did you forget to call the {nameof(Initialize)} method?");
            }

            if (slotIndex < 0 || slotIndex > MaxSlotsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(slotIndex), $"{nameof(slotIndex)} cannot be less than 0 or greater than {MaxSlotsCount} (was {slotIndex}).");
            }

            string saveFilePath = GetSaveFilePath(slotIndex);

            if (!File.Exists(saveFilePath))
            {
                throw new ArgumentException($"There is no data corresponding to save slot {slotIndex}.");
            }

            var reader = new SaveFileReader(saveFilePath);
            SaveData data = reader.Read();
            return data;
        }

        public void SaveData(SaveData saveData, int slotIndex, bool overwrite)
        {
            if (SaveFolder == null)
            {
                throw new InvalidOperationException($"{nameof(SaveSystem)} is not initilized. Did you forget to call the {nameof(Initialize)} method?");
            }

            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData));
            }

            if (slotIndex < 0 || slotIndex > MaxSlotsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(slotIndex), $"{nameof(slotIndex)} cannot be less than 0 or greater than {MaxSlotsCount} (was {slotIndex}).");
            }

            string saveFilePath = GetSaveFilePath(slotIndex);

            if (File.Exists(saveFilePath) && !overwrite)
            {
                throw new InvalidOperationException($"Unable to save to slot {slotIndex} because it already contains some data, " +
                                                    $"and '{nameof(overwrite)}' argument was set to false.");
            }

            var writer = new SaveFileWriter(saveFilePath);
            writer.Overwrite(saveData);
        }

        public bool TryGetMostRecentlyUsedSlot(out int slotIndex)
        {
            slotIndex = -1;
            if (SaveFolder == null)
            {
                return false;
            }

            DateTime mostRecentTime = DateTime.MinValue;
            for (int i = 0; i < MaxSlotsCount; i++)
            {
                string filepath = GetSaveFilePath(i);

                FileInfo fileInfo = new FileInfo(filepath);
                if (!fileInfo.Exists)
                {
                    continue;
                }

                if (mostRecentTime < fileInfo.LastWriteTime)
                {
                    slotIndex = i;
                    mostRecentTime = fileInfo.LastWriteTime;
                }
            }

            return slotIndex != -1;
        }

        private string GetSaveFilePath(int slotIndex)
        {
            string filename = $"{slotIndex}-game.save";
            string fullPath = Path.Combine(SaveFolder, filename);
            return fullPath;
        }
    }
}