using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Members;
using AutoBattler.EventManagers;
using AutoBattler.UI.ResultNotifications;
using AutoBattler.UI.PlayerInfo;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;
using AutoBattler.Data.Enums;

namespace AutoBattler.Managers
{
    public class GameManager : Manager<GameManager>, IDataPersistence
    {
        [Header("Components")]
        [SerializeField] private Player player;
        [SerializeField] private List<Bot> bots;
        [SerializeField] private ShopDatabase shopDb;
        [SerializeField] private GameObject UICanvas;

        [Header("Prefabs")]
        [SerializeField] private RoundResultNotification roundLostNotification;
        [SerializeField] private RoundResultNotification roundWonNotification;

        [Header("Parameters")]
        [SerializeField] private float checkBattleWaitTime = 0.5f;
        [SerializeField] private float endBattleWaitTime = 2.5F;
        [SerializeField] private int startGainGoldPerRound = 3;
        [SerializeField] private int maxGainGoldPerRound = 10;
        [SerializeField] private int damageForLose = 1;
        [SerializeField] private int roundsWonAmountForWin = 10;

        private RoundResultNotification currentNotification;

        public static bool IsPlayerWon { get; private set; }

        public int CurrentRound { get; private set; } = 1;
        public GameMode GameMode { get; private set; }

        public ShopDatabase ShopDb => shopDb;
        public int StartGainGoldPerRound => startGainGoldPerRound;
        public int MaxGainGoldPerRound => maxGainGoldPerRound;

        private void Start()
        {
            RunBotsRoundLogic();
            RewardMembers();

            DataPersistenceManager.Instance.LoadGame();
            GameMode = DataPersistenceManager.Instance.GameMode;

            bool isSoloMode = GameMode == GameMode.Solo ? true : false;

            SetActiveConfrontationModeObjects(!isSoloMode);
            UIPlayerInfo.Instance.SetActiveSoloModeObjects(isSoloMode);
            UIPlayerInfo.Instance.SetActiveConfrontationModeObjects(!isSoloMode);

            BattleManager.Instance.Setup(player, bots);
            CameraMovement.Instance.CalculateBackgroundParameters();
        }

        private void SetActiveConfrontationModeObjects(bool isActive)
        {
            bots.ForEach(b => b.gameObject.SetActive(isActive));
        }

        public void OpenMenu()
        {
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadScene(0);
        }

        public void StartBattle()
        {
            switch (GameMode)
            {
                case GameMode.Solo:
                    StartSoloModeBattle();
                    break;
                case GameMode.Confrontation:
                    StartConfrontationModeBattle();
                    break;
            }
        }

        private void StartSoloModeBattle()
        {
            BattleManager.Instance.StartSoloModeBattle();
            FightEventManager.SendFightStarted();
            StartCoroutine(CheckSoloModeBattleStatusCoroutine());
        }

        private void StartConfrontationModeBattle()
        {
            BattleManager.Instance.StartConfrontationModeBattle();
            FightEventManager.SendFightStarted();
            StartCoroutine(CheckConfrontationModeBattleStatusCoroutine());
        }

        private IEnumerator CheckSoloModeBattleStatusCoroutine()
        {
            while (BattleManager.Instance.IsBothMemberArmiesAlive(player))
                yield return checkBattleWaitTime;

            if (BattleManager.Instance.IsMemberArmyAlive(player))
                PlayerWonBattle();
            else if (BattleManager.Instance.IsMemberEnemyArmyAlive(player))
                PlayerLostBattle();

            StartCoroutine(EndBattleCoroutine());
        }

        private IEnumerator CheckConfrontationModeBattleStatusCoroutine()
        {
            while (!BattleManager.Instance.IsConfrontationModeBattlesEnded())
                yield return checkBattleWaitTime;

            Dictionary<Member, bool> fightResults = BattleManager.Instance.GetFightResults();

            Member member;
            foreach (var fightResult in fightResults)
            {
                member = fightResult.Key;

                if (member is Player)
                {
                    if (fightResult.Value)
                        PlayerWonBattle();
                    else
                        PlayerLostBattle();
                    continue;
                }


                if (fightResult.Value)
                {
                    member.IncreaseRoundsWonAmountByOne();
                }
                else
                {
                    member.TakeDamage(damageForLose);
                }
            }

            StartCoroutine(EndBattleCoroutine());
        }

        private void PlayerLostBattle()
        {
            currentNotification = Instantiate(roundLostNotification, UICanvas.transform);
            (currentNotification as RoundLostNotification).Setup(
                player.GetRoundRewardGoldAmount(),
                -damageForLose
                );

            player.TakeDamage(damageForLose);
        }

        private void PlayerWonBattle()
        {
            currentNotification = Instantiate(roundWonNotification, UICanvas.transform);
            (currentNotification as RoundWonNotification).Setup(
                player.GetRoundRewardGoldAmount()
                );

            player.IncreaseRoundsWonAmountByOne();
        }

        private IEnumerator EndBattleCoroutine()
        {
            ++CurrentRound;
            
            yield return new WaitForSeconds(endBattleWaitTime);

            if (GameMode == GameMode.Solo)
                BattleManager.Instance.EndSoloModeBattle();
            else if (GameMode == GameMode.Confrontation)
                BattleManager.Instance.EndConfrontationModeBattle();

            FightEventManager.SendFightEnded();

            if (IsGameEnded())
            {
                LoadResultScene();
            }
            else
            {
                currentNotification?.Show();
                RewardMembers();
                RunBotsRoundLogic();
            }
        }

        private void RunBotsRoundLogic()
        {
            bots.ForEach(b => b.MakeTurn(CurrentRound, 0));
        }

        private void RewardMembers()
        {
            player.GainGold(player.GetRoundRewardGoldAmount());
            bots.ForEach(b => b.GetRoundRewardGoldAmount());
        }

        private bool IsGameEnded()
        {
            if (player.Health <= 0)
                return true;

            if (GameMode == GameMode.Solo && 
                player.RoundsWonAmount >= roundsWonAmountForWin)
                return true;

            if (GameMode == GameMode.Confrontation)
            {

            }

            return false;
        }

        private void LoadResultScene()
        {
            IsPlayerWon = player.Health > 0;
            SceneManager.LoadScene(2);
        }

        public void LoadData(GameData data)
        {
            GameMode = data.gameMode;
            CurrentRound = data.currentRound;
        }

        public void SaveData(GameData data)
        {
            data.gameMode = GameMode;
            data.currentRound = CurrentRound;
        }
    }
}
