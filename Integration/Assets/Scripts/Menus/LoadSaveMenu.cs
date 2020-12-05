using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.SaveSystems;
using UnityEngine;

namespace Assets.Scripts.Menus
{
    public class LoadSaveMenu : Menu
    {
        public GameObject saveSlotUiPrefab;

        private ISaveSystem _saveSystem;

        void Start()
        {
            for (int i = 0; i < _saveSystem.MaxSlotsCount; i++)
            {
                var saveData = _saveSystem.LoadData(i);
                Instantiate(saveSlotUiPrefab);
            }
        }

        public void OnSaveSlotSelected(int saveSlot)
        {
            PlayerPrefs.SetInt("SaveSlot", saveSlot);
            base.LoadMainScene(SceneId.MasterScene);
        }
    }
}