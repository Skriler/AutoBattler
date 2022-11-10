using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.UI.PlayerInfo;

namespace AutoBattler.UI.Manual
{
    public class UIManual : UIPanel
    {
        [Header("Components")]
        [SerializeField] private Button manualButton;
        [SerializeField] private List<GameObject> pages;

        private int currentPageIndex = 0;

        protected void Awake()
        {
            FightEventManager.OnFightStarted += HideManualButton;
            FightEventManager.OnFightEnded += ShowManualButton;
        }

        protected void OnDestroy()
        {
            FightEventManager.OnFightStarted -= HideManualButton;
            FightEventManager.OnFightEnded -= ShowManualButton;
        }

        private void OnEnable()
        {
            Time.timeScale = 0;
            CameraMovement.Instance.IsOnUI = true;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            CameraMovement.Instance.IsOnUI = false;
        }

        private void HideManualButton() => manualButton.gameObject.SetActive(false);

        private void ShowManualButton() => manualButton.gameObject.SetActive(true);

        public void Show() => gameObject.SetActive(!gameObject.activeSelf);

        public void GoToNextPage() => GoToPage(currentPageIndex + 1);

        public void GoToPreviousPage() => GoToPage(currentPageIndex - 1);

        public void GoToPage(int index)
        {
            if (index < 0 || index >= pages.Count)
                return;

            pages[currentPageIndex].gameObject.SetActive(false);
            pages[index].gameObject.SetActive(true);

            currentPageIndex = index;
        }
    }
}
