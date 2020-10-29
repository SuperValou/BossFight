using System;
using Assets.Scripts.Players.Inputs;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs.WindowStates
{
    public class RecordingState : IWindowState
    {
        private readonly string _inputFilePath;
        private readonly int _fps;
        private readonly string _outputFolderPath;

        private bool _isInitialized = false;

        private AbstractInput _characterControllerInput;
        private AbstractInput _weaponManagerInput;
        
        public RecordingState(string inputFilePath, int fps, string outputFolderPath)
        {
            _inputFilePath = inputFilePath ?? throw new ArgumentNullException(nameof(inputFilePath));
            _outputFolderPath = outputFolderPath ?? throw new ArgumentNullException(nameof(outputFolderPath));

            if (fps < 1)
            {
                throw new ArgumentException($"{nameof(fps)} cannot be less than 1.");
            }

            _fps = fps;
        }

        public WindowStateName Name => WindowStateName.Recording;

        public IWindowState OnGui()
        {
            if (!_isInitialized)
            {
                Initialize();
                return this;
            }

            throw new System.NotImplementedException();
        }

        private void Initialize()
        {
            _isInitialized = true;
            throw new System.NotImplementedException();
        }
    }
}