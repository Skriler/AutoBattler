using AutoBattler.Data.Members;
using AutoBattler.Managers;
using AutoBattler.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AutoBattler.UI.Menu
{
    public class UIResultMenu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject playerWonObjects;
        [SerializeField] private GameObject playerLostObjects;

        private void Start()
        {
            FileSaveSystem.DeleteSavedProgress();

            SetupSceneComponents(GameManager.IsPlayerWon);
        }

        public void LoadMainMenuScene() => SceneManager.LoadScene(0);

        public void RestartGame()
        {
            DataPersistenceManager.Instance.NewGame(DataPersistenceManager.Instance.GameMode);
            SceneManager.LoadScene(1);
        }

        private void SetupSceneComponents(bool isPlayerWon)
        {
            playerWonObjects.SetActive(isPlayerWon);
            playerLostObjects.SetActive(!isPlayerWon);
        }
    }
}
