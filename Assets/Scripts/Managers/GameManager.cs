using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Player;
using AutoBattler.EventManagers;

namespace AutoBattler.Managers
{
    public class GameManager : Manager<GameManager>
    {
        [Header("Components")]
        [SerializeField] private Player player;
        [SerializeField] private ShopDatabase shopDb;

        [Header("Parameters")]
        [SerializeField] private float endBattleWaitTime = 3;
        [SerializeField] private int maxGainGoldPerRound = 10;
        [SerializeField] private int damageForLose = 1;

        private BattleManager battleManager;

        public bool IsFightMode { get; private set; } = false;
        public int CurrentRound { get; private set; } = 1;

        public ShopDatabase ShopDb => shopDb;

        private void Update()
        {
            if (!IsFightMode)
                return;

            if (!battleManager.IsFirstArmyAlive())
            {
                Debug.Log("Player lost!");
                player.TakeDamage(damageForLose);
                StartCoroutine(EndBattleCoroutine());
            }
            else if (!battleManager.IsSecondArmyAlive())
            {
                Debug.Log("Player won!");
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

        public void OpenManual()
        {

        }

        private IEnumerator EndBattleCoroutine()
        {
            ++CurrentRound;
            IsFightMode = false;

            yield return new WaitForSeconds(endBattleWaitTime);

            battleManager.EndBattle();
            FightEventManager.SendFightEnded();

            RewardPlayer();
        }

        private void RewardPlayer()
        {
            int goldAmount = CurrentRound;
            goldAmount = goldAmount <= maxGainGoldPerRound ? goldAmount : maxGainGoldPerRound;
            player.GainGold(goldAmount);
        }
    }
}
