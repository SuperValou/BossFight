using System;
using System.IO;
using Assets.Scripts.Players.Inputs.ToCheck.Serializers;
using Assets.Scripts.Players.Inputs.ToCheck.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.ToCheck
{
    public class InputRecorder : MonoBehaviour
    {
        // -- Editor
        public AbstractInputManager inputManagerToRecord;

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

            frame.LookVector = inputManagerToRecord.GetLookVector();
            frame.MoveVector = inputManagerToRecord.GetMoveVector();

            frame.FireDown = inputManagerToRecord.FireButtonDown();
            frame.Fire = inputManagerToRecord.FireButton();
            frame.FireUp = inputManagerToRecord.FireButtonUp();

            frame.JumpDown = inputManagerToRecord.JumpButtonDown();
            frame.Jump = inputManagerToRecord.JumpButton();

            frame.BoosterDown = inputManagerToRecord.BoosterButtonDown();

            frame.DashDown = inputManagerToRecord.DashButtonDown();

            frame.SwitchWeaponDown = inputManagerToRecord.SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection);
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