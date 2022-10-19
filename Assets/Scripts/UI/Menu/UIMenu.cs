using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace AutoBattler.UI.Menu
{
    public class UIMenu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private TextMeshProUGUI textVersion;

        private bool isOptionsMenuOpened = false;

        private void Start()
        {
            textVersion.text += Application.version;
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenOptions()
        {
            isOptionsMenuOpened = !isOptionsMenuOpened;

            mainMenu.SetActive(!isOptionsMenuOpened);
            optionsMenu.SetActive(isOptionsMenuOpened);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
