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
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button previousPageButton;
        [SerializeField] private List<GameObject> pages;

        private int currentPageIndex = 0;
        private float initialTimeScale = 1;

        private void Start()
        {
            GoToPage(0);
        }

        private void OnEnable()
        {
            initialTimeScale = Time.timeScale;
            Time.timeScale = 0;
            CameraMovement.Instance.IsOnUI = true;
        }

        private void OnDisable()
        {
            Time.timeScale = initialTimeScale;
            CameraMovement.Instance.IsOnUI = false;
        }

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

            bool IsNextPageButtonActive = index == pages.Count - 1 ? false : true;
            bool IsPreviousPageButtonActive = index == 0 ? false : true;
            nextPageButton.gameObject.SetActive(IsNextPageButtonActive);
            previousPageButton.gameObject.SetActive(IsPreviousPageButtonActive);
        }
    }
}
