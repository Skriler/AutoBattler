using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Player;
using AutoBattler.EventManagers;
using AutoBattler.UI.ResultNotifications;
using AutoBattler.SaveSystem;

namespace AutoBattler.Managers
{
    public class GameManager : Manager<GameManager>, IDataPersistence
    {
        [Header("Components")]
        [SerializeField] private Player player;
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

        public ShopDatabase ShopDb => shopDb;
        public int MaxGainGoldPerRound => maxGainGoldPerRound;

        public void OpenMenu()
        {
            //DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadScene(0);
        }

        public void StartBattle()
        {
            battleManager = new BattleManager(player.Field, player.EnemyField, shopDb);
            battleManager.StartBattle();
            FightEventManager.SendFightStarted();

            StartCoroutine(CheckBattleStatusCoroutine());
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
        }

        public void LoadData(GameData data)
        {
            CurrentRound = data.CurrentRound;
        }

        public void SaveData(GameData data)
        {
            data.CurrentRound = CurrentRound;
        }
    }
}
