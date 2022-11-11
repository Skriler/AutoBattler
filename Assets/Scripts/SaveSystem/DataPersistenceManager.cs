using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Managers;

namespace AutoBattler.SaveSystem
{
    public class DataPersistenceManager : Manager<DataPersistenceManager>
    {
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;

        private void Start()
        {
            dataPersistenceObjects = GetAllDataPersistenceObjects();

            //LoadGame();
        }

        public void NewGame()
        {
            gameData = new GameData();
        }

        public void LoadGame()
        {
            gameData = FileSaveSystem.LoadProgress();

            if (gameData == null)
            {
                NewGame();
                return;
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                dataPersistenceObj.LoadData(gameData);
        }

        public void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                dataPersistenceObj.SaveData(gameData);

            FileSaveSystem.SaveProgress(gameData);
        }

        public List<IDataPersistence> GetAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);

        }
    }
}
