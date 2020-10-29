using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs.WindowStates
{
    public class SettingUpState : IWindowState
    {
        private string _inputFilePath;
        private int _fps;
        private string _outputFolderPath;
        
        public WindowStateName Name => WindowStateName.SettingUp;

        public IWindowState OnGui()
        {
            GUILayout.Label("Input file path");
            //_inputFilePath = GUILayout.TextField(_inputFilePath);

            GUILayout.Label("FPS");
            _fps = EditorGUILayout.IntField(_fps);

            GUILayout.Label("Output folder");
            _outputFolderPath = GUILayout.TextField(_outputFolderPath);

            if (GUILayout.Button("Start Capture"))
            {
                if (!File.Exists(_inputFilePath))
                {
                    EditorUtility.DisplayDialog("Error", "Input file path does not exist.", "Ok");
                    return this;
                }

                if (!Directory.Exists(_outputFolderPath))
                {
                    EditorUtility.DisplayDialog("Error", "Output folder does not exist.", "Ok");
                    return this;
                }

                RecordingState recordingState = new RecordingState(_inputFilePath, _fps, _outputFolderPath);
                return recordingState;
            }

            if (GUILayout.Button("Encode Video"))
            {
                if (!Directory.Exists(_outputFolderPath))
                {
                    EditorUtility.DisplayDialog("Error", "Output folder does not exist.", "Ok");
                    return this;
                }

                IWindowState encodingState = new EncodingState(_outputFolderPath, _fps);
                return encodingState;
            }

            return this;
        }
    }
}