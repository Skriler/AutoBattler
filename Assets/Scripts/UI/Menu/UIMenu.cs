using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using AutoBattler.SaveSystem;
using AutoBattler.Managers;
using AutoBattler.SaveSystem.Data;
using AutoBattler.Data.Enums;

namespace AutoBattler.UI.Menu
{
    public class UIMenu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject gameModeSelectionMenu;
        [SerializeField] private GameObject leaderboardMenu;
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI textVersion;
        [SerializeField] private TextMeshProUGUI textShadowVersion;
        [SerializeField] private Image uiBackground;

        private void Start()
        {
            Time.timeScale = 1;

            textVersion.text += Application.version;
            textShadowVersion.text += Application.version;

            FileSaveSystem.LoadSettings();
            SetPlayerSettings();

            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            gameModeSelectionMenu.SetActive(false);

            continueButton.interactable = FileSaveSystem.IsSavedProgress();
        }

        public void QuitGame() => Application.Quit();

        private void SetPlayerSettings()
        {
            AudioManager.Instance.SetVolume("MasterVolume", PlayerSettings.MasterVolume);
            AudioManager.Instance.SetVolume("MusicVolume", PlayerSettings.MusicVolume);
            AudioManager.Instance.SetVolume("EffectsVolume", PlayerSettings.EffectsVolume);
            AudioManager.Instance.SetVolume("UIVolume", PlayerSettings.UIVolume);

            Screen.fullScreen = PlayerSettings.IsFullScreen;
            Screen.SetResolution(
                PlayerSettings.Resolution.width, 
                PlayerSettings.Resolution.height, 
                PlayerSettings.IsFullScreen, 
                PlayerSettings.Resolution.refreshRate
                );
        }

        public void StartNewSoloModeGame()
        {
            FileSaveSystem.DeleteSavedProgress();
            DataPersistenceManager.Instance.NewGame(GameMode.Solo);
            LoadGameScene();
        }

        public void StartNewConfrontationModeGame()
        {
            FileSaveSystem.DeleteSavedProgress();
            DataPersistenceManager.Instance.NewGame(GameMode.Confrontation);
            LoadGameScene();
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenGameModeSelectionMenu() => OpenMenu(gameModeSelectionMenu);

        public void OpenLeaderboardMenu() => OpenMenu(leaderboardMenu);

        public void OpenOptionsMenu()
        {
            OpenMenu(optionsMenu);
            continueButton.interactable = FileSaveSystem.IsSavedProgress();
        }

        private void OpenMenu(GameObject menu)
        {
            mainMenu.SetActive(menu.activeSelf);
            uiBackground.gameObject.SetActive(!menu.activeSelf);
            menu.SetActive(!menu.activeSelf);
        }
    }
}
