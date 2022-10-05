using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.Units
{
    public class FireWizard : BaseUnit
    {
        protected List<BaseUnit> currentTargets;

        protected void Start()
        {
            currentTargets = new List<BaseUnit>();
        }

        protected override bool HasTarget() => currentTargets.Count != 0;

        protected override void DealDamageToTarget()
        {
            if (!HasTarget())
                return;

            foreach (BaseUnit target in currentTargets)
                target.TakeDamage(AttackDamage, DamageType);
        }

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
            if (IsEnemyMode)
            {
                if (oddUnits.Count < evenUnits.Count)
                    targets.AddRange(evenUnits);
                else
                    targets.AddRange(oddUnits);
            }
            else
            {
                if (evenUnits.Count < oddUnits.Count)
                    targets.AddRange(oddUnits);
                else
                    targets.AddRange(evenUnits);
            }
        }
    }
}
