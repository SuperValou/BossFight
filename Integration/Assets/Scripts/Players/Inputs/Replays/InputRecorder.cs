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

        // -- Class

        private InputFrameWriter _writer;

        private bool _isRecording;

        void Start()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found at \"{filePath}\"");
            }

            _writer = new InputFrameWriter(filePath);

            StartRecording();
        }

        void Update()
        {
            if (_isRecording)
            {
                RecordFrame();
            }
        }
        
        void OnDestroy()
        {
            StopRecording();
        }

        private void StartRecording()
        {
            _writer.Open();
            _isRecording = true;
        }

        private void RecordFrame()
        {
            var frame = new InputFrame();

            frame.Time = Time.time;

            frame.LookVector = inputToRecord.GetLookVector();
            frame.MoveVector = inputToRecord.GetMoveVector();

            frame.FireDown = inputToRecord.FireButtonDown();
            frame.Fire = inputToRecord.FireButton();
            frame.FireUp = inputToRecord.FireButtonUp();

            frame.JumpDown = inputToRecord.JumpButtonDown();
            frame.Jump = inputToRecord.JumpButton();

            frame.BoosterDown = inputToRecord.BoosterButtonDown();

            frame.DashDown = inputToRecord.DashButtonDown();

            frame.SwitchWeaponDown = inputToRecord.SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection);
            frame.WeaponSwitchDirection = (int) weaponSwitchDirection;

            _writer.WriteFrame(frame);
        }

        private void StopRecording()
        {
            _writer?.Close();
            _isRecording = false;
        }

    }
}