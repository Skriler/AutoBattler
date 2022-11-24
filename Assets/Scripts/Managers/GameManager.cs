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
using AutoBattler.Managers.Battle;

namespace AutoBattler.Managers
{
    public class GameManager : Manager<GameManager>, IDataPersistence
    {
        [Header("Components")]
        [SerializeField] private Player player;
        [SerializeField] private List<Bot> bots;
        [SerializeField] private ShopDatabase shopDb;
        [SerializeField] private GameObject UICanvas;
        [SerializeField] private BattleManager soloModeBattleManager;
        [SerializeField] private BattleManager confrontationModeBattleManager;

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

        private BattleManager currentBattleManager;
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

            HideAnotherGameModeObjects();

            currentBattleManager = GameMode switch
            {
                GameMode.Solo => soloModeBattleManager,
                GameMode.Confrontation => confrontationModeBattleManager,
                _ => soloModeBattleManager
            };
            currentBattleManager.Setup(player, bots);

            CameraMovement.Instance.CalculateBackgroundParameters();
        }

        private void HideAnotherGameModeObjects()
        {
            bool isSoloMode = GameMode == GameMode.Solo ? true : false;

            bots.ForEach(b => b.gameObject.SetActive(!isSoloMode));
            UIPlayerInfo.Instance.SetActiveSoloModeObjects(isSoloMode);
            UIPlayerInfo.Instance.SetActiveConfrontationModeObjects(!isSoloMode);
        }

        public void OpenMenu()
        {
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadScene(0);
        }

        public void StartBattle()
        {
            currentBattleManager.StartBattle();
            FightEventManager.SendFightStarted();
            StartCoroutine(CheckBattleStatusCoroutine());
        }

        private IEnumerator CheckBattleStatusCoroutine()
        {
            while (!currentBattleManager.IsBattleEnded())
                yield return checkBattleWaitTime;

            if (currentBattleManager.IsMemberArmyAlive(player))
                CreatePlayerWonBattleNotification();
            else
                CreatePlayerLostBattleNotification();

            Dictionary<Member, bool> fightResults = currentBattleManager.GetFightResults();

            Member member;
            foreach (var fightResult in fightResults)
            {
                member = fightResult.Key;

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

        private void CreatePlayerLostBattleNotification()
        {
            currentNotification = Instantiate(roundLostNotification, UICanvas.transform);
            (currentNotification as RoundLostNotification).Setup(
                player.GetRoundRewardGoldAmount(),
                -damageForLose
                );
        }

        private void CreatePlayerWonBattleNotification()
        {
            currentNotification = Instantiate(roundWonNotification, UICanvas.transform);
            (currentNotification as RoundWonNotification).Setup(
                player.GetRoundRewardGoldAmount()
                );
        }

        private IEnumerator EndBattleCoroutine()
        {
            ++CurrentRound;
            
            yield return new WaitForSeconds(endBattleWaitTime);

            currentBattleManager.EndBattle();
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
