using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.Menus
{
    public class MainMenu : Menu
    {
        public void Play()
        {
            base.LoadMainScene(SceneId.MasterScene);
        }
    }
}