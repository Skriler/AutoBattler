using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AutoBattler.Data.Enums;
using AutoBattler.Managers;

namespace AutoBattler.UI.Menu
{
    public class UILeaderboardMenu : Manager<UILeaderboardMenu>
    {
        [Header("Components")]
        [SerializeField] private GameObject soloModeContentContainer;
        [SerializeField] private GameObject confrontationModeContentContainer;
        [SerializeField] private TextMeshProUGUI leaderboardItem;

        [Header("Leaderboard parameters")]
        [SerializeField] private int maxScoreAmount = 30;

        private void Start()
        {
            SetLeaderboardItems(GameMode.Solo);
            SetLeaderboardItems(GameMode.Confrontation);
        }

        private void SetLeaderboardItems(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Solo:
                    ScoreManager.Instance.GetScoreList(GameMode.Solo, maxScoreAmount);
                    break;
                case GameMode.Confrontation:
                    ScoreManager.Instance.GetScoreList(GameMode.Confrontation, maxScoreAmount);
                    break;
            }
        }

        public void InstantiateLeaderboardItems(Dictionary<int, int> scoreDictionary, GameMode gameMode)
        {
            GameObject parentContainer = gameMode switch
            {
                GameMode.Solo => soloModeContentContainer,
                GameMode.Confrontation => confrontationModeContentContainer,
                _ => soloModeContentContainer
            };

            TextMeshProUGUI leaderboardItemText;
            foreach (KeyValuePair<int, int> scoreEntry in scoreDictionary)
            {
                leaderboardItemText = Instantiate(leaderboardItem, parentContainer.transform);
                leaderboardItemText.text = scoreEntry.Key + ". " + scoreEntry.Value;
            }
        }
    }
}
