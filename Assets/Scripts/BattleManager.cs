using System;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Units;
using AutoBattler.UnitBoxes;

public class BattleManager
{
    private FieldManager firstArmyField;
    private ShopDatabase shopDb;

    private BaseUnit[,] firstArmy;
    private BaseUnit[,] secondArmy;

    public BattleManager(FieldManager firstArmyField, ShopDatabase shopDb)
    {
        this.firstArmyField = firstArmyField;
        this.shopDb = shopDb;

        firstArmy = firstArmyField.GetArmy();
        secondArmy = new BaseUnit[firstArmy.GetLength(0), firstArmy.GetLength(1)];

        GenerateSecondArmy();
        firstArmyField.SpawnSecondArmy(secondArmy);
        FlipSecondArmyUnitsSpritesOnX();
    }

    private void StartBattle()
    {
        for (int i = 0; i < firstArmy.GetLength(0); ++i)
        {
            for (int j = 0; j < firstArmy.GetLength(1); ++j)
            {
                if (firstArmy[i, j] != null)
                {
                    firstArmy[i, j].Attack();
                }

                if (secondArmy[i, j] != null)
                {
                    secondArmy[i, j].Attack();
                }
            }
        }
    }

    private void GenerateSecondArmy()
    {
        List<ShopDatabase.ShopUnit> shopUnits = shopDb.GetUnits();
        int shopUnitsAmount = shopDb.GetUnitsAmount();
        int unitsAmount = firstArmyField.GetUnitsAmount();

        for (int i = 0; i < firstArmy.GetLength(0); ++i)
        {
            for (int j = 0; j < firstArmy.GetLength(1); ++j)
            {
                if (i * firstArmy.GetLength(1) + j >= unitsAmount)
                    continue;

                secondArmy[i, j] = shopUnits[UnityEngine.Random.Range(0, shopUnitsAmount)].prefab;
            }
        }
    }

    private void FlipSecondArmyUnitsSpritesOnX()
    {
        for (int i = 0; i < firstArmy.GetLength(0); ++i)
        {
            for (int j = 0; j < firstArmy.GetLength(1); ++j)
            {
                if (secondArmy[i, j] == null)
                    continue;

                secondArmy[i, j].FlipOnX();
            }
        }
    }
}
