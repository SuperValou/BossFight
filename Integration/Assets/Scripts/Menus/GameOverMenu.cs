using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.Menus
{
    public class GameOverMenu : Menu
    {
        public void Retry()
        {
            base.LoadMainScene(SceneId.MasterScene);
        }

        public void Quit()
        {
            base.LoadMainScene(SceneId.MainMenu);
        }
    }
}