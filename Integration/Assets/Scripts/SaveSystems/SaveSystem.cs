using System;
using System.IO;
using Assets.Scripts.SaveSystems.Serializers.DTOs;

namespace Assets.Scripts.SaveSystems
{
    public class SaveSystem : ISaveSystem
    {
        public string SaveFolder { get; }
        public int MaxSlotsCount { get; }
        
        public SaveSystem(string saveFolder, int maxSlotsCount)
        {
            if (maxSlotsCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxSlotsCount));
            }

            if (!Directory.Exists(saveFolder))
            {
                throw new ArgumentException($"{nameof(saveFolder)} not found at \"{saveFolder}\".");
            }

            SaveFolder = saveFolder;
            MaxSlotsCount = maxSlotsCount;
        }
        
        public bool IsFree(int slotIndex)
        {
            string filepath = GetSaveFilePath(slotIndex);
            bool saveFileExists = File.Exists(filepath);
            return !saveFileExists;
        }

        public SaveData LoadData(int slotIndex)
        {
            throw new System.NotImplementedException();
        }

        public void SaveData(SaveData saveData, int slotIndex, bool overwrite)
        {
            throw new System.NotImplementedException();
        }

        public int GetMostRecentlyUsedSlot()
        {
            int mostRecentIndex = 0;
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
                    mostRecentIndex = i;
                    mostRecentTime = fileInfo.LastWriteTime;
                }
            }

            return mostRecentIndex;
        }

        private string GetSaveFilePath(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > MaxSlotsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(slotIndex), $"{nameof(slotIndex)} cannot be less than 0 or greater than {MaxSlotsCount} (was {slotIndex}).");
            }

            string filename;
            if (slotIndex == 0)
            {
                filename = "0-newgame.save";
            }
            else
            {
                filename = $"{slotIndex}-game.save";
            }

            string fullPath = Path.Combine(SaveFolder, filename);
            return fullPath;
        }
    }
}