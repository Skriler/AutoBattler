using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

namespace AutoBattler.Managers
{
    public class LeaderboardManager : Manager<LeaderboardManager>
    {
        [Header("Parameters")]
        [SerializeField] private int leaderboardId;

        protected override void Awake()
        {
            if (Instance != null)
                return;

            base.Awake();
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            LootLockerSDKManager.StartSession("Player", (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Failed");
                }
            });
        }

        public void GetScoreList()
        {
            LootLockerSDKManager.GetScoreList(leaderboardId, 5, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Success GetScoreList");
                }
                else
                {
                    Debug.Log("Failed GetScoreList");
                }
            });
        }

        public void SubmitScore()
        {
            //LootLockerSDKManager.SubmitScore(MemberId.text, int.Parse(PlayerScore.text), id, (response) =>
            //{
            //    if (response.success)
            //    {
            //        Debug.Log("Success SubmitScore");
            //    }
            //    else
            //    {
            //        Debug.Log("Failed SubmitScore");
            //    }
            //});
        }
    }
}
