using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Managers;
using AutoBattler.EventManagers;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.SaveSystem
{
    public class DataPersistenceManager : Manager<DataPersistenceManager>
    {
        [SerializeField] private bool useEncryption = true;
        [SerializeField] private string encryptionCodeWord = "SomeWord";

        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;

        protected override void Awake()
        {
            if (Instance != null)
                return;

            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void NewGame()
        {
            gameData = new GameData();
        }

        public void LoadGame()
        {
            dataPersistenceObjects = GetAllDataPersistenceObjects();
            //gameData = FileSaveSystem.LoadProgress(useEncryption, encryptionCodeWord);

            if (gameData == null)
            {
                NewGame();
                return;
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                dataPersistenceObj.LoadData(gameData);

            SaveSystemEventManager.SendDataLoaded();
        }

        public void SaveGame()
        {
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
