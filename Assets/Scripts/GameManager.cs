using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private ShopDatabase shopDb;

    public ShopDatabase GetShopDb() => shopDb;

    public void StartBattle()
    {
        BattleManager battleManager = new BattleManager(player.Field, shopDb);
    }
}
