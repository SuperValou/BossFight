namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    public enum SceneId
    {
        [SceneInfo("MainMenu", SceneType.Screen)]
        MainMenu,

        [SceneInfo("MasterScene", SceneType.Master)]
        MasterScene,

        [SceneInfo("0-Gameplay", SceneType.Gameplay)]
        GameplayScene,

        [SceneInfo("1-BossRoom", SceneType.Room)]
        BossRoomScene,
    }
}