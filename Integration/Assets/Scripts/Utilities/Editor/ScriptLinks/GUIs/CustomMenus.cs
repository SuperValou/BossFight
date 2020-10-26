using UnityEditor;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.GUIs
{
    public static class CustomMenus
    {
        [MenuItem("Tools/Script Link")]
        public static void ShowMissingScriptsWindow()
        {
            EditorWindow.GetWindow<ScriptLinkWindow>();
        }
    }
}