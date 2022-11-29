using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private GameObject UICanvas;
        [SerializeField] private BattleManager soloModeBattleManager;
        [SerializeField] private BattleManager confrontationModeBattleManager;

        [Header("Prefabs")]
        [SerializeField] private RoundLostNotification roundLostNotification;
        [SerializeField] private RoundWonNotification roundWonNotification;

        [Header("Parameters")]
        [SerializeField] private float checkBattleWaitTime = 0.5f;
        [SerializeField] private float endBattleWaitTime = 2.5F;

        [Header("Characteristics")]
        [SerializeField] private int startGainGoldPerRound = 3;
        [SerializeField] private int maxGainGoldPerRound = 10;
        [SerializeField] private int damageForLose = 1;
        [SerializeField] private int goldenCupAmountForWin = 10;
        [SerializeField] private int goldenCupAmountForRoundWin = 1;

        [Header("TimeScale")]
        [SerializeField] private float timeScaleStep = 0.25f;
        [SerializeField] private float minTimeScale = 1f;
        [SerializeField] private float maxTimeScale = 3.5f;

        private BattleManager currentBattleManager;
        private RoundResultNotification currentNotification;

        public static bool IsPlayerWon { get; private set; }

        public int CurrentRound { get; private set; } = 1;
        public GameMode GameMode { get; private set; }

        public int StartGainGoldPerRound => startGainGoldPerRound;
        public int MaxGainGoldPerRound => maxGainGoldPerRound;

        private void Start()
        {
            RunBotsRoundLogic();

            DataPersistenceManager.Instance.LoadGame();
            InitializeFields();

            HideAnotherGameModeObjects();
            CameraMovement.Instance.CalculateBackgroundParameters();
        }

        private void InitializeFields()
        {
            GameMode = DataPersistenceManager.Instance.GameMode;
            goldenCupAmountForWin = 
                PlayerSettings.MaxGoldenCupAmount <= 0 ?
                goldenCupAmountForWin : 
                PlayerSettings.MaxGoldenCupAmount;

            currentBattleManager = GameMode switch
            {
                GameMode.Solo => soloModeBattleManager,
                GameMode.Confrontation => confrontationModeBattleManager,
                _ => soloModeBattleManager
            };
            currentBattleManager.Setup(player, bots);
        }

        private void HideAnotherGameModeObjects()
        {
            bool isSoloMode = GameMode == GameMode.Solo ? true : false;

            bots.ForEach(b => b.gameObject.SetActive(!isSoloMode));
            UIPlayerInfo.Instance.SetActiveSoloModeObjects(isSoloMode);
            UIPlayerInfo.Instance.SetActiveConfrontationModeObjects(!isSoloMode);
            roundWonNotification.SetActiveGoldenCupContainer(isSoloMode);
        }

        public void IncreaseTimeScale() => ChangeTimeScale(Time.timeScale + timeScaleStep);
        public void DecreaseTimeScale() => ChangeTimeScale(Time.timeScale - timeScaleStep);

        private void ChangeTimeScale(float value)
        {
            if (value < minTimeScale || value > maxTimeScale)
                return;

            Time.timeScale = value;
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
                    member.IncreaseGoldenCupAmount(goldenCupAmountForRoundWin);
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
                player.GetRoundRewardGoldAmount(),
                goldenCupAmountForRoundWin
                );
        }

        private IEnumerator EndBattleCoroutine()
        { 
            yield return new WaitForSeconds(endBattleWaitTime);

            currentBattleManager.EndBattle();
            FightEventManager.SendFightEnded();

            CheckAndRemoveDeadBots();

            if (IsGameEnded())
            {
                LoadResultScene();
            }
            else
            {
                currentNotification?.Show();
                currentBattleManager?.Setup(player, bots);

                RewardMembers();
                ++CurrentRound;

                if (GameMode == GameMode.Confrontation)
                    RunBotsRoundLogic();

                DataPersistenceManager.Instance.SaveGame();
            }
        }

        private void RunBotsRoundLogic()
        {
            bots.ForEach(b => b.MakeTurn());
        }

        private void RewardMembers()
        {
            player.GainGold(player.GetRoundRewardGoldAmount());
            bots.ForEach(b => b.GainGold(b.GetRoundRewardGoldAmount()));
        }

        private bool IsGameEnded()
        {
            if (player.Health <= 0)
                return true;

            if (GameMode == GameMode.Solo && player.GoldenCup >= goldenCupAmountForWin)
                return true;

            if (GameMode == GameMode.Confrontation && bots.Count == 0)
                return true;

            return false;
        }

        private void CheckAndRemoveDeadBots()
        {
            List<Bot> botForDelete = new List<Bot>();

            foreach (Bot bot in bots)
            {
                if (bot.IsAlive())
                    continue;

                botForDelete.Add(bot);
                bot.Death();
            }

            botForDelete.ForEach(b => bots.Remove(b));
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
