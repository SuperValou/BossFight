using System.IO;
using Assets.Scripts.Players.Inputs.Replays.Serializers;
using Assets.Scripts.Players.Inputs.Replays.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.Replays
{
    public class InputReplayer : AbstractInput
    {
        // -- Editor

        public string inputFile = "<to set>";

        // -- Class

        private InputFrameReader _reader;

        private InputFrame _currentFrame = new InputFrame();

        void Start()
        {
            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException($"Input file doesn't exist at '{inputFile}'.");
            }

            _reader = new InputFrameReader(inputFile);
            _reader.Open();

            // read first frame
            _currentFrame = _reader.ReadFrame();
            Time.captureDeltaTime = _currentFrame.Time;
        }

        void LateUpdate()
        {
            if (!_reader.CanRead())
            {
                Time.captureDeltaTime = 0;
                return;
            }
            
            var nextFrame = _reader.ReadFrame();

            Time.captureDeltaTime = nextFrame.Time - _currentFrame.Time;
            _currentFrame = nextFrame;
        }

        void OnDestroy()
        {
            _reader?.Close();
        }

        // -- Input overrides 

        public override Vector2 GetLookVector()
        {
            return _currentFrame.LookVector;
        }

        public override Vector3 GetMoveVector()
        {
            return _currentFrame.MoveVector;
        }

        public override bool FireButtonDown()
        {
            return _currentFrame.FireDown;
        }

        public override bool FireButton()
        {
            return _currentFrame.Fire;
        }

        public override bool FireButtonUp()
        {
            return _currentFrame.FireUp;
        }

        public override bool JumpButton()
        {
            return _currentFrame.Jump;
        }

        public override bool JumpButtonDown()
        {
            return _currentFrame.JumpDown;
        }

        public override bool BoosterButtonDown()
        {
            return _currentFrame.BoosterDown;
        }

        public override bool DashButtonDown()
        {
            return _currentFrame.DashDown;
        }

        public override bool SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection)
        {
            weaponSwitchDirection = (WeaponSwitchDirection) _currentFrame.WeaponSwitchDirection;
            return _currentFrame.SwitchWeaponDown;
        }

        public override bool LockOnButtonDown()
        {
            throw new System.NotImplementedException();
        }
    }
}