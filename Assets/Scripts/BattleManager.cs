using System;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.ScriptableObjects;

public class BattleManager
{
    private FieldContainer fieldContainer;
    private ShopDatabase shopDb;

    private BaseUnit[,] firstArmy;
    private BaseUnit[,] secondArmy;

    public BattleManager(FieldContainer fieldContainer, ShopDatabase shopDb)
    {
        this.fieldContainer = fieldContainer;
        this.shopDb = shopDb;

        firstArmy = fieldContainer.GetArmy();
        secondArmy = new BaseUnit[firstArmy.GetLength(0), firstArmy.GetLength(1)];

        GenerateSecondArmy();
        fieldContainer.SpawnSecondArmy(secondArmy);
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
        int shopUnitsAmount = shopUnits.Count;
        int unitsAmount = fieldContainer.GetUnitsAmount();

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
}
