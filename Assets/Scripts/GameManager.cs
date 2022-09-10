using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Players;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private ShopDatabase shopDb;
    [SerializeField] private BuffDatabase buffDb;

    public ShopDatabase ShopDb => shopDb;
    public BuffDatabase BuffDb => buffDb;

    public void StartBattle()
    {
        BattleManager battleManager = new BattleManager(player.Field, shopDb);
    }
}
