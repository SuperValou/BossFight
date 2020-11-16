using System;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.SaveSystems
{
    public class SaveManager : MonoBehaviour
    {
        public SceneLoadingManager sceneLoadingManager;

        private ISaveSystem _saveSystem = null;

        void Start()
        {
            int saveSlot = PlayerPrefs.GetInt("SaveSlot");
            
            
        }

        public void LoadGame(int saveSlot)
        {
            var save = _saveSystem.LoadData(saveSlot);
            if (!Enum.TryParse(save.saveRoomId, out SceneId sceneId))
            {
                // throw
            }

            sceneLoadingManager.LoadSubSenesAsync(sceneId);
            sceneLoadingManager.LoadSubSenesAsync(SceneId.GameplayScene);

            var player = GameObject.FindObjectOfType<Player>();
            player.transform.position = save.playerPosition;
            player.transform.rotation = save.playerRotation;
            player.FirstPersonController.hasJumpAbility = save.powerUps.jump;
        }

        public void SaveGame()
        {
            int saveSlot = PlayerPrefs.GetInt("SaveSlot");
        }

        public void SaveGame(int saveSlot)
        {
            throw new NotImplementedException();
        }
    }
}