using System.Collections;
using Assets.Scripts.Players;
using Assets.Scripts.SaveSystems;
using UnityEngine;

namespace Assets.Scripts.MainScenesScripts
{
    public class MasterSceneScript : MonoBehaviour
    {
        // -- Editor

        public int defaultSaveSlotToLoadFrom;

        [Header("References")]
        public SaveManager saveManager;

        // -- Class

        private Player _player;

        void Awake()
        {
            saveManager.PreferedSaveSlot = defaultSaveSlotToLoadFrom;
        }

        IEnumerator Start()
        {
            yield return saveManager.LoadGame();
        }
    }
}