using System;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.Players;
using Assets.Scripts.Players.Inputs;
using Assets.Scripts.Players.Inputs.Replays;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs
{
    public class ReplayCaptureWindow : EditorWindow
    {
        private FirstPersonController _firstPersonController;
        private WeaponManager _weaponManager;
        private InputRecorder _inputRecorder;
        private InputReplayer _inputReplayer;
        private ScreenshotCaptureManager _screenshotCaptureManager;

        private AbstractInput _charaControllerInput;
        private AbstractInput _weaponInput;

        private int _tabIndex;
        private readonly string[] _tabs = new[] {"Normal Game", "Record Inputs", "Replay Inputs", "Capture Replay"};

        void Awake()
        {
            string sceneName = SceneInfo.GetAll().First(s => s.Id == SceneId.GameplayScene).Name;
            this.titleContent = new GUIContent($"Replay Capture ({sceneName})");
            
            _firstPersonController = Resources.FindObjectsOfTypeAll<FirstPersonController>().FirstOrDefault(o => o.gameObject.scene.name == sceneName);
            _weaponManager = Resources.FindObjectsOfTypeAll<WeaponManager>().FirstOrDefault(o => o.gameObject.scene.name == sceneName);
            
            _inputRecorder = Resources.FindObjectsOfTypeAll<InputRecorder>().FirstOrDefault(o => o.gameObject.scene.name == sceneName); Object.FindObjectOfType<InputRecorder>();
            _inputReplayer = Resources.FindObjectsOfTypeAll<InputReplayer>().FirstOrDefault(o => o.gameObject.scene.name == sceneName);
            _screenshotCaptureManager = Resources.FindObjectsOfTypeAll<ScreenshotCaptureManager>().FirstOrDefault(o => o.gameObject.scene.name == sceneName);
        }
        
        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            int newTabIndex = GUILayout.Toolbar(_tabIndex, _tabs);
            if (newTabIndex == _tabIndex)
            {
                return;
            }

            _tabIndex = newTabIndex;
            Reset();

            switch (_tabIndex)
            {
                case 0: // Normal Game
                    break;

                case 1: // Record Inputs
                    SetRecordInputMode();
                    break;

                case 2: // Replay Inputs
                    SetReplayInputMode();
                    break;

                case 3: // Capture Replay
                    SetCaptureReplayMode();
                    break;
            }
        }

        private void SetRecordInputMode()
        {
            if (_inputRecorder == null)
            {
                throw new InvalidOperationException($"Unable to find {nameof(InputRecorder)}.");
            }

            _inputRecorder.gameObject?.SetActive(true);
        }

        private void SetReplayInputMode()
        {
            if (_inputReplayer == null)
            {
                throw new InvalidOperationException($"Unable to find {nameof(InputReplayer)}.");
            }

            if (_firstPersonController == null)
            {
                throw new InvalidOperationException($"Unable to find {nameof(FirstPersonController)}.");
            }

            if (_weaponManager == null)
            {
                throw new InvalidOperationException($"Unable to find {nameof(FirstPersonController)}.");
            }

            _firstPersonController.input = _inputReplayer;
            _weaponManager.input = _inputReplayer;
            _inputReplayer.gameObject?.SetActive(true);
        }

        private void SetCaptureReplayMode()
        {
            if (_screenshotCaptureManager == null)
            {
                throw new InvalidOperationException($"Unable to find {nameof(ScreenshotCaptureManager)}.");
            }

            SetReplayInputMode();
            
            _screenshotCaptureManager.gameObject.SetActive(true);
        }

        private void Reset()
        {
            _inputRecorder?.gameObject?.SetActive(false);
            _inputReplayer?.gameObject?.SetActive(false);
            _screenshotCaptureManager?.gameObject?.SetActive(false);

            if (_firstPersonController != null)
            {
                _firstPersonController.input = _charaControllerInput;
            }
            
            if (_weaponManager != null)
            {
                _weaponManager.input = _weaponInput;
            }
        }

        void OnDestroy()
        {
            Reset();
        }
    }
}