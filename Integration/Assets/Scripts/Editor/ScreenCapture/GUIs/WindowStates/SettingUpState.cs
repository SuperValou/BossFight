using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs.WindowStates
{
    public class SettingUpState : IWindowState
    {
        private string _inputFilePath;
        private int _fps = 25;
        private string _screenshotFolderPath;
        
        public WindowStateName Name => WindowStateName.SettingUp;

        public IWindowState OnGui()
        {
            GUILayout.Label("Recorded inputs file path");
            _inputFilePath = GUILayout.TextField("<not available yet>");

            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);

            GUILayout.Label("Screenshots folder");
            _screenshotFolderPath = GUILayout.TextField(_screenshotFolderPath);

            GUILayout.Label("FPS");
            _fps = EditorGUILayout.IntField(_fps);

            if (GUILayout.Button("<not available yet> Start Gameplay Capture"))
            {
                EditorUtility.DisplayDialog("Error", "This feature is not available yet.", "Ok");

                if (!File.Exists(_inputFilePath))
                {
                    EditorUtility.DisplayDialog("Error", "Input file path does not exist.", "Ok");
                    return this;
                }

                if (!Directory.Exists(_screenshotFolderPath))
                {
                    EditorUtility.DisplayDialog("Error", "Output folder does not exist.", "Ok");
                    return this;
                }

                RecordingState recordingState = new RecordingState(_inputFilePath, _fps, _screenshotFolderPath);
                return recordingState;
            }

            if (GUILayout.Button("Encode Video"))
            {
                if (!Directory.Exists(_screenshotFolderPath))
                {
                    EditorUtility.DisplayDialog("Error", "Output folder does not exist.", "Ok");
                    return this;
                }

                IWindowState encodingState = new EncodingState(_screenshotFolderPath, _fps);
                return encodingState;
            }

            return this;
        }
    }
}