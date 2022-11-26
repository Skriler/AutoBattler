using System.Collections.Generic;

namespace AutoBattler.Data.Units
{
    public class MultipleTargetsMage : MultipleTargetsUnit
    {
        protected override void FindTarget(BaseUnit[,] enemyUnits)
        {
            if (enemyUnits == null)
                return;

            List<BaseUnit> evenUnits = new List<BaseUnit>();
            List<BaseUnit> oddUnits = new List<BaseUnit>();

            for (int i = 0; i < enemyUnits.GetLength(0); ++i)
            {
                for (int j = 0; j < enemyUnits.GetLength(1); ++j)
                {
                    if (!IsUnitAlive(enemyUnits[i, j]))
                        continue;

                    if ((i + j) % 2 == 0)
                        evenUnits.Add(enemyUnits[i, j]);
                    else
                        oddUnits.Add(enemyUnits[i, j]);
                }
            }

            currentTargets.Clear();
            DetermineOptimalTarget(evenUnits, oddUnits, ref currentTargets);
        }

        protected void DetermineOptimalTarget(List<BaseUnit> evenUnits, List<BaseUnit> oddUnits, ref List<BaseUnit> targets)
        {
            if (oddUnits.Count < evenUnits.Count)
                targets.AddRange(evenUnits);
            else
                targets.AddRange(oddUnits);
        }
    }
}
