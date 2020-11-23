namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    public enum SceneId
    {
        [SceneInfo("MainMenu", SceneType.Screen)]
        MainMenu,

        [SceneInfo("GameOverMenu", SceneType.Screen)]
        GameOverMenu,

        [SceneInfo("MasterScene", SceneType.Master)]
        MasterScene,

        [SceneInfo("0-Gameplay", SceneType.Gameplay)]
        GameplayScene,

        [SceneInfo("1-FirstRoom", SceneType.Room)]
        FirstRoomScene,

        [SceneInfo("1-JumpRoom", SceneType.Room)]
        JumpRoomScene,

        [SceneInfo("1-SaveStationRoom", SceneType.Room)]
        SaveStationRoomScene,

        [SceneInfo("1-BossRoom", SceneType.Room)]
        BossRoomScene,

        [SceneInfo("AbilityRoomScene", SceneType.TestRoom)]
        AbilityRoomScene
    }
}