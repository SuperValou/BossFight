using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

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