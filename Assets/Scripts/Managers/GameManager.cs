using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Player;
using AutoBattler.EventManagers;
using AutoBattler.UI.ResultNotifications;
using AutoBattler.UI.Effects;

namespace AutoBattler.Managers
{
    public class GameManager : Manager<GameManager>
    {
        [Header("Components")]
        [SerializeField] private Player player;
        [SerializeField] private ShopDatabase shopDb;
        [SerializeField] private GameObject UICanvas;

        [Header("Prefabs")]
        [SerializeField] private RoundResultNotification roundLostNotification;
        [SerializeField] private RoundResultNotification roundWonNotification;

        [Header("Parameters")]
        [SerializeField] private float endBattleWaitTime = 3;
        [SerializeField] private int maxGainGoldPerRound = 10;
        [SerializeField] private int damageForLose = 1;

        private BattleManager battleManager;
        private RoundResultNotification currentNotification;

        public bool IsFightMode { get; private set; } = false;
        public int CurrentRound { get; private set; } = 1;

        public ShopDatabase ShopDb => shopDb;
        public int MaxGainGoldPerRound => maxGainGoldPerRound;

        private void Update()
        {
            if (!IsFightMode)
                return;

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

        public void StartBattle()
        {
            IsFightMode = true;

            battleManager = new BattleManager(player.Field, player.EnemyField, shopDb);
            battleManager.StartBattle();
            FightEventManager.SendFightStarted();
        }

        public void OpenMenu()
        {
            SceneManager.LoadScene(0);
        }

        private IEnumerator EndBattleCoroutine()
        {
            ++CurrentRound;
            IsFightMode = false;

            yield return new WaitForSeconds(endBattleWaitTime);

            battleManager.EndBattle();
            FightEventManager.SendFightEnded();

            player.GainGold(player.GetRoundRewardGoldAmount());
            currentNotification.Show();
        }
    }
}
