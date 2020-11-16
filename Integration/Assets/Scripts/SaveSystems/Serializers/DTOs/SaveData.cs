using System;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

namespace Assets.Scripts.SaveSystems.Serializers.DTOs
{
    [Serializable]
    public class SaveData
    {
        public int slotIndex;

        public DateTime lastUsed;
        public string user;

        public string saveRoomId;
        public Vector3 playerPosition;
        public Quaternion playerRotation;

        public PowerUpData powerUps;
    }
}