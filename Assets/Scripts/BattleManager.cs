using System.Collections.Generic;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.ScriptableObjects.Databases;

public class BattleManager
{
    private FieldContainer fieldContainer;
    private FieldContainer enemyFieldContainer;
    private ShopDatabase shopDb;

    private BaseUnit[,] firstArmy;
    private BaseUnit[,] secondArmy;

    private int ArmyWidth;
    private int ArmyHeight;

    public BattleManager(FieldContainer fieldContainer, FieldContainer enemyFieldContainer, ShopDatabase shopDb)
    {
        this.fieldContainer = fieldContainer;
        this.enemyFieldContainer = enemyFieldContainer;
        this.shopDb = shopDb;

        firstArmy = fieldContainer.GetArmy();
        secondArmy = enemyFieldContainer.GetArmy();

        ArmyWidth = firstArmy.GetLength(0);
        ArmyHeight = firstArmy.GetLength(1);

        GenerateSecondArmy();
        enemyFieldContainer.SpawnUnits(secondArmy);
    }

    public BaseUnit[,] GetSecondArmy() => secondArmy;

    public void StartBattle()
    {
        for (int i = 0; i < ArmyWidth; ++i)
        {
            for (int j = 0; j < ArmyHeight; ++j)
            {
                firstArmy[i, j]?.EnterFightMode(secondArmy);
                secondArmy[i, j]?.EnterFightMode(firstArmy);
            }
        }
    }

    public void EndBattle()
    {
        for (int i = 0; i < ArmyWidth; ++i)
        {
            for (int j = 0; j < ArmyHeight; ++j)
            {
                firstArmy[i, j]?.ExitFightMode();
                secondArmy[i, j]?.ExitFightMode();
            }
        }
    }

    private void GenerateSecondArmy()
    {
        List<ShopUnitEntity> shopUnits = shopDb.GetUnits();
        int shopUnitsAmount = shopUnits.Count;
        int unitsAmount = fieldContainer.GetUnitsAmount();

        for (int i = 0; i < ArmyWidth; ++i)
        {
            for (int j = 0; j < ArmyHeight; ++j)
            {
                if (i * ArmyHeight + j >= unitsAmount)
                    continue;

                secondArmy[i, j] = shopUnits[UnityEngine.Random.Range(0, shopUnitsAmount)].prefab;
            }
        }
    }

    public bool IsFirstArmyAlive() => fieldContainer.IsAtLeastOneAliveUnit();

    public bool IsSecondArmyAlive()
    {
        for (int i = 0; i < secondArmy.GetLength(0); ++i)
        {
            for (int j = 0; j < secondArmy.GetLength(1); ++j)
            {
                if (secondArmy[i, j] == null)
                    continue;

                if (secondArmy[i, j].IsAlive())
                    return true;
            }
        }

        return false;
    }
}
