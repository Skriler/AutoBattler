using System;
using System.Collections;
using System.Collections.Generic;

public class BattleManager
{
    private static Random rand = new Random();

    private BaseUnit[,] firstArmy;
    private BaseUnit[,] secondArmy;

    public BattleManager(int maxWidth, int maxHeight)
    {
        firstArmy = new BaseUnit[maxWidth, maxHeight];
        secondArmy = new BaseUnit[maxWidth, maxHeight];
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
        int unitsAmount = rand.Next(4, 6);

        for (int i = 0; i < unitsAmount; ++i)
        {

        }
    }
}
