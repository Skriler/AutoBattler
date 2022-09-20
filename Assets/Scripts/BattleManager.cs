using System.Collections.Generic;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.ScriptableObjects.Databases;

public class BattleManager
{
    private FieldContainer fieldContainer;
    private ShopDatabase shopDb;

    private BaseUnit[,] firstArmy;
    private BaseUnit[,] secondArmy;

    private int ArmyWidth;
    private int ArmyHeight;

    public BattleManager(FieldContainer fieldContainer, ShopDatabase shopDb)
    {
        this.fieldContainer = fieldContainer;
        this.shopDb = shopDb;

        ArmyWidth = fieldContainer.GetArmyWidth();
        ArmyHeight = fieldContainer.GetArmyHeight();

        firstArmy = fieldContainer.GetArmy();
        secondArmy = new BaseUnit[ArmyWidth, ArmyHeight];

        GenerateSecondArmy();
        fieldContainer.SpawnEnemyUnits(secondArmy);
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
