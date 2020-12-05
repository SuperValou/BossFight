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
            Time.captureDeltaTime = _currentFrame.t;
        }

        void LateUpdate()
        {
            if (!_reader.CanRead())
            {
                Time.captureDeltaTime = 0;
                return;
            }
            
            var nextFrame = _reader.ReadFrame();

            Time.captureDeltaTime = nextFrame.t - _currentFrame.t;
            _currentFrame = nextFrame;
        }

        void OnDestroy()
        {
            _reader?.Close();
        }

        // -- Input overrides 

        public override Vector2 GetLookVector()
        {
            return _currentFrame.look;
        }

        public override Vector3 GetMoveVector()
        {
            return _currentFrame.move;
        }

        public override bool FireButtonDown()
        {
            return _currentFrame.fireDw;
        }

        public override bool FireButton()
        {
            return _currentFrame.fire;
        }

        public override bool FireButtonUp()
        {
            return _currentFrame.fireUp;
        }

        public override bool JumpButton()
        {
            return _currentFrame.jump;
        }

        public override bool JumpButtonDown()
        {
            return _currentFrame.jumpDw;
        }

        public override bool BoosterButtonDown()
        {
            return _currentFrame.boostDw;
        }

        public override bool DashButtonDown()
        {
            return _currentFrame.dashDw;
        }

        public override bool SwitchWeaponDown(out WeaponSwitchDirection weaponSwitchDirection)
        {
            weaponSwitchDirection = (WeaponSwitchDirection) _currentFrame.weapSwtchDir;
            return _currentFrame.swtchWeapDw;
        }

        public override bool LockOnButtonDown()
        {
            return _currentFrame.lockonDw;
        }
    }
}