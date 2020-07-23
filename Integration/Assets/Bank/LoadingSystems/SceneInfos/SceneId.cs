namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    public enum SceneId
    {
        [SceneInfo("MasterScene", SceneType.Master)]
        MasterScene = 0,

        [SceneInfo("0-Gameplay", SceneType.Gameplay)]
        GameplayScene,

        [SceneInfo("1-BossRoom", SceneType.Room)]
        BossRoomScene,
    }
}