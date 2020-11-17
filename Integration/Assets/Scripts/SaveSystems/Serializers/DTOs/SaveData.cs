using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystems.Serializers.DTOs
{
    [Serializable]
    public class SaveData
    {
        public DateTime date;
        public string username;

        public string saveRoomId;
        public Vector3 playerPosition;
        public Quaternion playerRotation;

        public PowerUpData powerUps;
    }
}