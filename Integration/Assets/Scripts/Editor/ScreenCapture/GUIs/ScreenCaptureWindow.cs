using System;
using Assets.Scripts.Editor.ScreenCapture.GUIs.WindowStates;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs
{
    public class ScreenCaptureWindow : EditorWindow
    {
        private IWindowState _currentState;
        
        void Awake()
        {
            this.titleContent = new GUIContent("Screen Capture");

            _currentState = new SettingUpState();
        }
        
        void OnGUI()
        {
            if (_currentState == null)
            {
                GUILayout.Label("Something went wrong. Check the logs for more details.");
                return;
            }

            IWindowState nextState;
            try
            {
                nextState = _currentState.OnGui();
            }
            catch (Exception)
            {
                _currentState = null;
                throw;
            }

            _currentState = nextState;
        }
    }
}