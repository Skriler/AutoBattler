using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Managers;
using AutoBattler.EventManagers;
using AutoBattler.SaveSystem.Data;
using AutoBattler.Data.Enums;

namespace AutoBattler.SaveSystem
{
    public class DataPersistenceManager : Manager<DataPersistenceManager>
    {
        [SerializeField] private bool useEncryption = true;
        [SerializeField] private string encryptionCodeWord = "SomeWord";

        public GameMode GameMode { get; private set; }

        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;

        protected override void Awake()
        {
            if (Instance != null)
                return;

            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void NewGame(GameMode gameMode)
        {
            GameMode = gameMode;
            gameData = new GameData();
            SaveSystemEventManager.SendNewGameDataCreated();
        }

        public void LoadGame()
        {
            dataPersistenceObjects = GetAllDataPersistenceObjects();
            gameData = FileSaveSystem.LoadProgress(useEncryption, encryptionCodeWord);

            if (gameData == null)
            {
                NewGame(GameMode);
                return;
            }

            GameMode = gameData.gameMode;

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                dataPersistenceObj.LoadData(gameData);

            SaveSystemEventManager.SendDataLoaded();
        }

        public void SaveGame()
        {
            dataPersistenceObjects = GetAllDataPersistenceObjects();
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                dataPersistenceObj.SaveData(gameData);

            FileSaveSystem.SaveProgress(gameData, useEncryption, encryptionCodeWord);
        }

        public List<IDataPersistence> GetAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);

        }
    }
}
