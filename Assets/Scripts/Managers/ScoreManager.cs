using System;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using AutoBattler.Data.Enums;
using AutoBattler.UI.Menu;

namespace AutoBattler.Managers
{
    public class ScoreManager : Manager<ScoreManager>
    {
        [Header("Score parameters")]
        [SerializeField] private int scoreMultiplier = 10;

        [Header("Leaderboard parameters")]
        [SerializeField] private int soloModeLeaderboardId = 9204;
        [SerializeField] private int confrontationModeLeaderboardId = 9220;

        public int Score { get; private set; } = 0;

        private string currentId; 

        protected override void Awake()
        {
            if (Instance != null)
                return;

            base.Awake();
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            currentId = Guid.NewGuid().ToString("N");

            LootLockerSDKManager.StartSession(currentId, (response) => { });
        }

        public void IncreaseScore(int value)
        {
            Score += value * scoreMultiplier;
        }

        public Dictionary<int, int> GetScoreList(GameMode gameMode, int maxScoreAmount)
        {
            int leaderboardId = gameMode switch
            {
                GameMode.Solo => soloModeLeaderboardId,
                GameMode.Confrontation => confrontationModeLeaderboardId,
                _ => 0
            };

            Dictionary<int, int> scoreDictionary = new Dictionary<int, int>();

            LootLockerSDKManager.GetScoreList(leaderboardId, maxScoreAmount, (response) =>
            {
                if (response.success)
                {
                    LootLockerLeaderboardMember[] scores = response.items;

                    for (int i = 0; i < scores.Length; ++i)
                        scoreDictionary.Add(scores[i].rank, scores[i].score);

                    UILeaderboardMenu.Instance.InstantiateLeaderboardItems(scoreDictionary, gameMode);
                }
            });

            return scoreDictionary;
        }

        public void SubmitScore(GameMode gameMode)
        {
            int leaderboardId = gameMode switch
            {
                GameMode.Solo => soloModeLeaderboardId,
                GameMode.Confrontation => confrontationModeLeaderboardId,
                _ => 0
            };

            LootLockerSDKManager.SubmitScore(currentId, Score, leaderboardId, (response) => { });
        }
    }
}
