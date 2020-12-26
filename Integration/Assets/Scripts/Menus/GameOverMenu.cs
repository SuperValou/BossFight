using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

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