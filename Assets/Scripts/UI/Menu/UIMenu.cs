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
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI textVersion;
        [SerializeField] private Image uiBackground;

        private void Start()
        {
            textVersion.text += Application.version;

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

        public void OpenOptionsMenu()
        {
            mainMenu.SetActive(optionsMenu.activeSelf);
            uiBackground.gameObject.SetActive(!optionsMenu.activeSelf);
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }

        public void OpenGameModeSelectionMenu()
        {
            mainMenu.SetActive(gameModeSelectionMenu.activeSelf);
            uiBackground.gameObject.SetActive(!gameModeSelectionMenu.activeSelf);
            gameModeSelectionMenu.SetActive(!gameModeSelectionMenu.activeSelf);
        }
    }
}
