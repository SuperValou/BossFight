using System.Collections.Generic;
using Assets.Scripts.SaveSystems.Serializers.DTOs;

namespace Assets.Scripts.SaveSystems
{
    public interface ISaveSystem
    {
        int MaxSlotsCount { get; }

        bool IsFree(int slotIndex);

        SaveData LoadData(int slotIndex);

        void SaveData(SaveData saveData, int slotIndex, bool overwrite);

        int GetMostRecentlyUsedSlot();
    }
}