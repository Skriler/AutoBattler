using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int maxGainGoldPerRound = 10;
        [SerializeField] private int damageForLose = 1;

        private RoundResultNotification currentNotification;

        public int CurrentRound { get; private set; } = 1;
        public GameMode GameMode { get; private set; } = GameMode.Solo;

        public ShopDatabase ShopDb => shopDb;
        public int MaxGainGoldPerRound => maxGainGoldPerRound;

        private void Start()
        {
            GameMode = DataPersistenceManager.Instance.GameMode;

            bool isSoloMode = GameMode == GameMode.Solo ? true : false;

            SetActiveConfrontationModeObjects(!isSoloMode);
            UIPlayerInfo.Instance.SetActiveSoloModeObjects(isSoloMode);
            UIPlayerInfo.Instance.SetActiveConfrontationModeObjects(!isSoloMode);

            if (!isSoloMode)
            {
                RunBotsRoundLogic();
            }
            
            DataPersistenceManager.Instance.LoadGame();
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
            BattleManager.Instance.Setup(player.Field, player.EnemyField);
            BattleManager.Instance.StartBattle();
            FightEventManager.SendFightStarted();
            StartCoroutine(CheckBattleStatusCoroutine());
        }

        private IEnumerator CheckBattleStatusCoroutine()
        {
            while (BattleManager.Instance.IsFirstArmyAlive() && BattleManager.Instance.IsSecondArmyAlive())
                yield return checkBattleWaitTime;

            if (!BattleManager.Instance.IsFirstArmyAlive())
            {
                currentNotification = Instantiate(roundLostNotification, UICanvas.transform);
                (currentNotification as RoundLostNotification).Setup(
                    player.GetRoundRewardGoldAmount(),
                    -damageForLose
                    );

                player.TakeDamage(damageForLose);
                StartCoroutine(EndBattleCoroutine());
            }
            else if (!BattleManager.Instance.IsSecondArmyAlive())
            {
                currentNotification = Instantiate(roundWonNotification, UICanvas.transform);
                (currentNotification as RoundWonNotification).Setup(
                    player.GetRoundRewardGoldAmount()
                    );

                player.IncreaseRoundsWonAmountByOne();
                StartCoroutine(EndBattleCoroutine());
            }
        }

        private IEnumerator EndBattleCoroutine()
        {
            ++CurrentRound;

            yield return new WaitForSeconds(endBattleWaitTime);

            BattleManager.Instance.EndBattle();
            currentNotification.Show();
            player.GainGold(player.GetRoundRewardGoldAmount());

            FightEventManager.SendFightEnded();
        }

        private void RunBotsRoundLogic()
        {
            bots.ForEach(b => b.MakeTurn(CurrentRound, 0));
        }

        private void StartConfrontationModeBattle()
        {
            List<(Member, Member)> battlePairs = GenerateBattlePairs();


        }

        private List<(Member, Member)> GenerateBattlePairs()
        {
            List<(Member, Member)> battlePairs = new List<(Member, Member)>();
            List<Member> battleMembers = new List<Member>(bots);

            int battlePairsAmount = (bots.Count + 1) / 2;
            int memberIndex = Random.Range(0, battleMembers.Count - 1);

            battlePairs.Add((player, battleMembers[memberIndex]));
            battleMembers.RemoveAt(memberIndex);

            Member firstMember;
            Member secondMember;
            for (int i = 1; i < battlePairsAmount; ++i)
            {
                firstMember = battleMembers[Random.Range(0, battleMembers.Count - 1)];
                secondMember = battleMembers[Random.Range(0, battleMembers.Count - 1)];

                battlePairs.Add((firstMember, secondMember));

                battleMembers.Remove(firstMember);
                battleMembers.Remove(secondMember);
            }

            return battlePairs;
        }

        public void LoadData(GameData data)
        {
            CurrentRound = data.currentRound;
        }

        public void SaveData(GameData data)
        {
            data.currentRound = CurrentRound;
        }
    }
}
