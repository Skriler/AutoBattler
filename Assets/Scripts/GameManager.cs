using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Players;
using AutoBattler.Managers;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private ShopDatabase shopDb;

    public ShopDatabase ShopDb => shopDb;

    public void StartBattle()
    {
        BattleManager battleManager = new BattleManager(player.Field, shopDb);
    }
}
