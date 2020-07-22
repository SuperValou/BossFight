using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        public void Load()
        {
            // TODO: integrate scene loading repo
            SceneManager.LoadScene(0);
        }
    }
}