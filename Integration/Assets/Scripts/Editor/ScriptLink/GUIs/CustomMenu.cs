using UnityEditor;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.GUIs
{
    public static class CustomMenu
    {
        [MenuItem("Tools/Script Link")]
        public static void ShowMissingScriptsWindow()
        {
            EditorWindow.GetWindow<ScriptLinkWindow>();
        }
    }
}