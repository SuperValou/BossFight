using System;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.Replays.Serializers.DTOs
{
    [Serializable]
    public struct InputFrame
    {
        public float t;

        public Vector2 look;
        public Vector3 move;

        public bool lockonDw;

        public bool fireDw;
        public bool fire;
        public bool fireUp;

        public bool jumpDw;
        public bool jump;

        public bool boostDw;

        public bool dashDw;

        public bool swtchWeapDw;
        public int weapSwtchDir;
    };
}