using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.UnitBoxes;

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
