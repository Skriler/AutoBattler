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

    private void GenerateSecondArmy()
    {
        int unitsAmount = rand.Next(4, 6);

        for (int i = 0; i < unitsAmount; ++i)
        {

        }
    }
}
