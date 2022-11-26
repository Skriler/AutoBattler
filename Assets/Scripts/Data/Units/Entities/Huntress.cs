using System.Collections.Generic;

namespace AutoBattler.Data.Units.Entities
{
    public class Huntress : SingleTargetUnit
    {
        protected override void FindTarget(BaseUnit[,] enemyUnits)
        {
            List<BaseUnit> aliveUnits = new List<BaseUnit>();

            for (int i = 0; i < enemyUnits.GetLength(0); ++i)
            {
                for (int j = 0; j < enemyUnits.GetLength(1); ++j)
                {
                    if (!IsUnitAlive(enemyUnits[i, j]))
                        continue;

                    aliveUnits.Add(enemyUnits[i, j]);
                }
            }

            if (aliveUnits.Count >= 1)
                currentTarget = DetermineOptimalTarget(aliveUnits);
        }
    }
}
