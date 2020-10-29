namespace Assets.Scripts.Editor.ScreenCapture.GUIs.WindowStates
{
    public interface IWindowState
    {
        WindowStateName Name { get; }

        IWindowState OnGui();
    }
}