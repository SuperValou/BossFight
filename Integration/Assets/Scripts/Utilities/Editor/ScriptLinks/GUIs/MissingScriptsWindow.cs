using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.GUIs
{
    public class MissingScriptsWindow : EditorWindow
    {
        private ISceneInfoManager _manager;

        void Awake()
        {
            this.titleContent = new GUIContent("Missing Scripts");
            _manager = new SceneInfoManager();
        }

        void OnGUI()
        {
            if (!_manager.IsInitialized)
            {
                if (GUILayout.Button("Initialize with current scene"))
                {
                    _manager.InitializeWithCurrentScene();
                }

                return;
            }

            if (GUILayout.Button("Take scene snapshot"))
            {
                _manager.SnapshotSceneInfo();
            }
        }
    }
}