using System;
using System.Collections;
using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.Players;
using Assets.Scripts.SaveSystems.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.SaveSystems
{
    public class SaveManager : MonoBehaviour
    {
        // -- Editor

        public SceneLoadingManager sceneLoadingManager;

        public DoorManager doorManager;

        // -- Class

        private ISaveSystem _saveSystem;

        public int PreferedSaveSlot { get; set; }

        void Awake()
        {
            var saveSystem = new SaveSystem();
            saveSystem.Initialize();
            _saveSystem = saveSystem;
        }

        public IEnumerator LoadGame()
        {
            SaveData saveData;
            if (_saveSystem.IsFree(PreferedSaveSlot))
            {
                saveData = CreateNewSaveData();
            }
            else
            {
                saveData = _saveSystem.LoadData(PreferedSaveSlot);
            }
            
            if (!Enum.TryParse(saveData.saveRoomId, out SceneId sceneId))
            {
                throw new InvalidOperationException($"Unable to parse {nameof(SceneId)} '{sceneId}' from save slot {PreferedSaveSlot}. Did you forget to add it to the enumeration?");
            }

            yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.GameplayScene);
            var player = GameObject.FindObjectOfType<Player>();
            player.gameObject.SetActive(false);

            yield return sceneLoadingManager.LoadSubSenesAsync(sceneId);
            
            ApplySaveData(saveData, player);
            player.gameObject.SetActive(true);
        }

        public void RequestToSaveGame()
        {
            SaveData existingSaveData;
            if (_saveSystem.IsFree(PreferedSaveSlot))
            {
                existingSaveData = CreateNewSaveData();
            }
            else
            {
                existingSaveData = _saveSystem.LoadData(PreferedSaveSlot);
            }

            var player = GameObject.FindObjectOfType<Player>(); // TODO: avoid that
            UpdateSaveData(existingSaveData, player, doorManager.PlayerCurrentRoomId);

            // TODO: ask for confirmation
            _saveSystem.SaveData(existingSaveData, PreferedSaveSlot, overwrite: true);
        }

        private static SaveData CreateNewSaveData()
        {
            PowerUpData powerUp = new PowerUpData()
            {
                jump = false,
                powerGun = false
            };

            SaveData newGameData = new SaveData()
            {
                date = DateTime.Now,
                username = Environment.UserName,

                saveRoomId = SceneId.FirstRoomScene.ToString(),
                playerPosition = Vector3.zero,
                playerRotation = Quaternion.identity,

                powerUps = powerUp
            };

            return newGameData;
        }

        private static void ApplySaveData(SaveData saveData, Player playerToUpdate)
        {
            playerToUpdate.transform.position = saveData.playerPosition;
            playerToUpdate.transform.rotation = saveData.playerRotation;

            playerToUpdate.FirstPersonController.hasJumpAbility = saveData.powerUps.jump;
        }

        private static void UpdateSaveData(SaveData saveDataToUpdate, Player player, SceneId currentRoomId)
        {
            saveDataToUpdate.date = DateTime.Now;

            saveDataToUpdate.saveRoomId = currentRoomId.ToString();
            saveDataToUpdate.playerPosition = player.transform.position;
            saveDataToUpdate.playerRotation = player.transform.rotation;

            saveDataToUpdate.powerUps.jump = player.FirstPersonController.hasJumpAbility;
        }
    }
}