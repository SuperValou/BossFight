using UnityEditor;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.GUIs
{
    public static class CustomMenus
    {
        [MenuItem("Tools/Missing Scripts Window")]
        public static void ShowMissingScriptsWindow()
        {
            EditorWindow.GetWindow<MissingScriptsWindow>();
        }
    }
}