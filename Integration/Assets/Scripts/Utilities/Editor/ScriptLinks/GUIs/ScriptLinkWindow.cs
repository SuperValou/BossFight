using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.GUIs
{
    public class ScriptLinkWindow : EditorWindow
    {
        private ScriptLinkManager _manager;

        void Awake()
        {
            this.titleContent = new GUIContent("Script Link");
            _manager = new ScriptLinkManager();
            _manager.Initialize();
        }
        

        void OnGUI()
        {
            if (!_manager.IsInitialized)
            {
                GUILayout.Label("Initializing...");
                return;
            }

            foreach (var report in _manager.Reports)
            {
                if (!report.IsReady)
                {
                    GUILayout.Label($"{report.SceneName} - not ready");
                }
                else if (report.MissingScriptPaths.Count == 0)
                {
                    GUILayout.Label($"{report.SceneName} - {report.ScriptNames.Count} script(s)");
                }
                else
                {
                    GUILayout.Label($"WARNING - {report.SceneName} - {report.MissingScriptPaths.Count} scripts");
                    foreach (var missingScriptPath in report.MissingScriptPaths)
                    {
                        GUILayout.Label($"\t{missingScriptPath}");
                    }
                }
            }

            if (GUILayout.Button("Take snapshot (will skip scenes with missing scripts)"))
            {
                _manager.TakeSnapshot();
            }
        }
    }
}