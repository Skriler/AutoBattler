using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using AutoBattler.SaveSystem;
using AutoBattler.Managers;
using AutoBattler.Data.Player;

namespace AutoBattler.UI.Menu
{
    public class UIMenu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI textVersion;

        private void Start()
        {
            textVersion.text += Application.version;

            FileSaveSystem.LoadSettings();
            SetPlayerSettings();

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

        public void StartNewGame()
        {
            FileSaveSystem.DeleteSavedProgress();
            LoadGameScene();
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenOptions()
        {
            mainMenu.SetActive(optionsMenu.activeSelf);
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }
    }
}
