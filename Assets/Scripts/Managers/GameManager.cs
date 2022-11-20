using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Player;
using AutoBattler.EventManagers;
using AutoBattler.UI.ResultNotifications;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

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

        private BattleManager battleManager;
        private RoundResultNotification currentNotification;

        public int CurrentRound { get; private set; } = 1;
        public bool SoloMode { get; private set; } = false;

        public ShopDatabase ShopDb => shopDb;
        public int MaxGainGoldPerRound => maxGainGoldPerRound;

        private void Start()
        {
            RunBotsRoundLogic();
            DataPersistenceManager.Instance.LoadGame();
        }

        public void OpenMenu()
        {
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadScene(0);
        }

        public void StartBattle()
        {
            if (SoloMode)
            {
                battleManager = new BattleManager(player.Field, player.EnemyField, shopDb);
                battleManager.StartBattle();
                FightEventManager.SendFightStarted();
            }
            else
            {

            }

            StartCoroutine(CheckBattleStatusCoroutine());
        }

        private void RunBotsRoundLogic()
        {
            bots.ForEach(b => b.MakeTurn(CurrentRound, 0));
        }

        private IEnumerator CheckBattleStatusCoroutine()
        {
            while (battleManager.IsFirstArmyAlive() && battleManager.IsSecondArmyAlive())
                yield return checkBattleWaitTime;

            if (!battleManager.IsFirstArmyAlive())
            {
                currentNotification = Instantiate(roundLostNotification, UICanvas.transform);
                (currentNotification as RoundLostNotification).Setup(
                    player.GetRoundRewardGoldAmount(),
                    -damageForLose
                    );

                player.TakeDamage(damageForLose);
                StartCoroutine(EndBattleCoroutine());
            }
            else if (!battleManager.IsSecondArmyAlive())
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

            battleManager.EndBattle();
            currentNotification.Show();
            player.GainGold(player.GetRoundRewardGoldAmount());

            FightEventManager.SendFightEnded();

            RunBotsRoundLogic();
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
