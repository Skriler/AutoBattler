using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace AutoBattler.UI.Menu
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textVersion;

        private void Start()
        {
            textVersion.text += Application.version;
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
