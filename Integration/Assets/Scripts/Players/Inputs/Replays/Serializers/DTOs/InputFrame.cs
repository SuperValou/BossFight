using System;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.Replays.Serializers.DTOs
{
    [Serializable]
    public struct InputFrame
    {
        public float Time;

        public Vector2 LookVector;
        public Vector3 MoveVector;

        public bool FireDown;
        public bool Fire;
        public bool FireUp;

        public bool JumpDown;
        public bool Jump;

        public bool BoosterDown;

        public bool DashDown;

        public bool SwitchWeaponDown;
        public int WeaponSwitchDirection;
    };
}