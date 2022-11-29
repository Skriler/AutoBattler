using AutoBattler.Managers;
using AutoBattler.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace AutoBattler.UI.Menu
{
    public class UIResultMenu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject playerWonObjects;
        [SerializeField] private GameObject playerLostObjects;
        [SerializeField] private TextMeshProUGUI textScoreShadow;
        [SerializeField] private TextMeshProUGUI textScore;

        private void Start()
        {
            Time.timeScale = 1;

            FileSaveSystem.DeleteSavedProgress();

            SetupSceneComponents(GameManager.IsPlayerWon);
            ScoreManager.Instance.SubmitScore(
                DataPersistenceManager.Instance.GameMode
                );
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

            int finalScore = ScoreManager.Instance.Score;

            textScore.text += finalScore.ToString();
            textScoreShadow.text += finalScore.ToString();
        }
    }
}
