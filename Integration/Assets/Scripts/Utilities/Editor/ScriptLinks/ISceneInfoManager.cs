namespace Assets.Scripts.Utilities.Editor.ScriptLinks
{
    public interface ISceneInfoManager
    {
        bool IsInitialized { get; }

        void InitializeWithCurrentScene();

        void SnapshotSceneInfo();
    }
}