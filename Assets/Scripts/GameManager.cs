using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Players;
using AutoBattler.Managers;
using AutoBattler.Data.Units;
using System.Collections;

public class GameManager : Manager<GameManager>
{
    [Header("Components")]
    [SerializeField] private Player player;
    [SerializeField] private ShopDatabase shopDb;

    [Header("Parameters")]
    [SerializeField] private float endBattleWaitTime = 3;

    private BattleManager battleManager;

    public bool IsFightMode { get; private set; } = false;

    //public bool IsCameraMovementActive { get; private set; } = true;

    public ShopDatabase ShopDb => shopDb;

    private void Update()
    {
        if (!IsFightMode)
            return;

        if (!battleManager.IsFirstArmyAlive())
        {
            Debug.Log("Player lost!");
            IsFightMode = false;
            StartCoroutine(EndBattleCoroutine());
        }
        else if (!battleManager.IsSecondArmyAlive())
        {
            Debug.Log("Player won!");
            IsFightMode = false;
            StartCoroutine(EndBattleCoroutine());
        }
    }

    //public void DisableCameraMovement() => IsCameraMovementActive = false;

    //public void EnableCameraMovement() => IsCameraMovementActive = true;

    public void StartBattle()
    {
        IsFightMode = true;

        battleManager = new BattleManager(player.Field, player.EnemyField, shopDb);
        battleManager.StartBattle();
    }

    private IEnumerator EndBattleCoroutine()
    {
        yield return new WaitForSeconds(endBattleWaitTime);
        EndBattle();
    }

    public void EndBattle()
    {
        battleManager.EndBattle();
    }   
}
