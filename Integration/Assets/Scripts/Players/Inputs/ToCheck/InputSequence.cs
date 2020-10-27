using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.ToCheck
{
    public class InputSequence
    {
        // var names are reduced for smaller json
        public float t;         // time
        public List<KeyCode> gK;    // getKey
        public List<KeyCode> gKD;   // getKeyDown
        public List<KeyCode> gKU;   // getKeyUp
        public Vector3 mP;      // mousePosition
        public Vector3 mWP;     // mouseWorldPosition
        public Vector2 mSD;     // mouseScrollDelta
        public List<string> vB;     // virtual Button
        public List<string> vBD;    // virtual Button Down
        public List<string> vBU;    // virtual Button Up
        public List<float> vA;      // virtual Axis

        public void Init()
        {
            gK = new List<KeyCode>();
            gKD = new List<KeyCode>();
            gKU = new List<KeyCode>();
            mP = new Vector3();
            mWP = new Vector3();
            mSD = new Vector2();
            vB = new List<string>();
            vBD = new List<string>();
            vBU = new List<string>();
            vA = new List<float>();
        }
    }
}