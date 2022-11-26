using System.Collections.Generic;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.Units
{
    public abstract class SingleTargetWarrior : SingleTargetUnit
    {
        protected override void FindTarget(BaseUnit[,] enemyUnits)
        {
            List<BaseUnit> aliveUnits = new List<BaseUnit>();

            for (int i = enemyUnits.GetLength(0) - 1; i >= 0; --i)
            {
                for (int j = 0; j < enemyUnits.GetLength(1); ++j)
                {
                    if (!IsUnitAlive(enemyUnits[i, j]))
                        continue;

                    aliveUnits.Add(enemyUnits[i, j]);
                }

                if (aliveUnits.Count >= 1)
                {
                    currentTarget = DetermineOptimalTarget(aliveUnits);
                    return;
                }
            }
        }
    }
}
