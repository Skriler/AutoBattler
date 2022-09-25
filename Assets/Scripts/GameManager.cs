using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Players;
using AutoBattler.Managers;
using AutoBattler.Data.Units;
using System.Collections;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private ShopDatabase shopDb;
    [SerializeField] private float endBattleWaitTime = 3;

    private BattleManager battleManager;

    private bool isFightMode = false;

    //public bool IsCameraMovementActive { get; private set; } = true;

    public ShopDatabase ShopDb => shopDb;

    private void Update()
    {
        if (!isFightMode)
            return;

        if (!battleManager.IsFirstArmyAlive())
        {
            Debug.Log("Player lost!");
            StartCoroutine(EndBattleCoroutine());
        }
        else if (!battleManager.IsSecondArmyAlive())
        {
            Debug.Log("Player won!");
            StartCoroutine(EndBattleCoroutine());
        }
    }

    //public void DisableCameraMovement() => IsCameraMovementActive = false;

    //public void EnableCameraMovement() => IsCameraMovementActive = true;

    public void StartBattle()
    {
        isFightMode = true;

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
        isFightMode = false;

        battleManager.EndBattle();
    }   
}
