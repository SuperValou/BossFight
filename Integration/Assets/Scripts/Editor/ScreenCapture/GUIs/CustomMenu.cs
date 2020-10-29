using UnityEditor;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs
{
    public static class CustomMenu
    {
        [MenuItem("Tools/Screen Capture")]
        public static void ShowMissingScriptsWindow()
        {
            EditorWindow.GetWindow<ScreenCaptureWindow>();
        }
    }
}