using System.IO;
using Assets.Scripts.Players.Inputs.Replays.Serializers;
using Assets.Scripts.Players.Inputs.Replays.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.Replays
{
    public class InputRecorder : MonoBehaviour
    {
        // -- Editor

        public AbstractInput inputToRecord;

        public string filePath = "<to set>";

        private bool record = true;

        // -- Class

        private InputFrameWriter _writer;

        void Start()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found at \"{filePath}\"");
            }

            _writer = new InputFrameWriter(filePath);
            _writer.Open();
        }

        void Update()
        {
            if (record)
            {
                RecordFrame();
            }
        }
        
        void OnDestroy()
        {
            _writer?.Close();
            record = false;
        }

        private void RecordFrame()
        {
            var frame = new InputFrame();

            frame.t = Time.time;

            frame.look = inputToRecord.GetLookVector();
            frame.move = inputToRecord.GetMoveVector();

            frame.lockonDw = inputToRecord.LockOnButtonDown();
            
            frame.fireDw = inputToRecord.FireButtonDown();
            frame.fire = inputToRecord.FireButton();
            frame.fireUp = inputToRecord.FireButtonUp();

            frame.jumpDw = inputToRecord.JumpButtonDown();
            frame.jump = inputToRecord.JumpButton();

            frame.boostDw = inputToRecord.BoosterButtonDown();

            frame.dashDw = inputToRecord.DashButtonDown();

            frame.swtchWeapDw = inputToRecord.SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection);
            frame.weapSwtchDir = (int) weaponSwitchDirection;

            _writer.WriteFrame(frame);
        }
    }
}