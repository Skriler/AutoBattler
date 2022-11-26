using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.Units.Entities
{
    public class Samurai : MultipleTargetsUnit
    {
        [Header("Fight Parameters")]
        [SerializeField] protected int maxTargetsAmount = 3;

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

                if (aliveUnits.Count >= 1)
                {
                    currentTargets.Clear();
                    DetermineOptimalTarget(aliveUnits, ref currentTargets);
                    return;
                }
            }
        }

        protected void DetermineOptimalTarget(List<BaseUnit> units, ref List<BaseUnit> targets)
        {
            if (units.Count <= maxTargetsAmount)
            {
                targets.AddRange(units);
                return;
            }

            List<BaseUnit> optimalTargets = new List<BaseUnit>();
            DetermineLowHealthUnit(units, ref optimalTargets);

            if (optimalTargets.Count >= maxTargetsAmount)
            {
                targets.AddRange(optimalTargets);
                return;
            } 

            DetermineOppositeDamageTypeUnit(units, ref optimalTargets);

            if (optimalTargets.Count >= maxTargetsAmount)
            {
                targets.AddRange(optimalTargets);
                return;
            }  

            foreach (BaseUnit unit in optimalTargets)
                units.Remove(unit);

            DetermineLeastHealthUnits(ref units, maxTargetsAmount - optimalTargets.Count);

            targets.AddRange(optimalTargets);
            targets.AddRange(units);
        }

        protected void DetermineLowHealthUnit(List<BaseUnit> units, ref List<BaseUnit> targets)
        {
            if (units.Count + targets.Count <= maxTargetsAmount)
            {
                targets.AddRange(units);
                return;
            }

            List<BaseUnit> lowHealthUnits = new List<BaseUnit>();

            foreach (BaseUnit unit in units)
            {
                if (!unit.HasLowHealth() || targets.Contains(unit))
                    continue;

                lowHealthUnits.Add(unit);
            }

            if (lowHealthUnits.Count + targets.Count <= maxTargetsAmount)
            {
                targets.AddRange(lowHealthUnits);
                return;
            }
                
            DetermineLeastHealthUnits(ref lowHealthUnits, maxTargetsAmount - targets.Count);
            targets.AddRange(lowHealthUnits);
        }

        protected void DetermineOppositeDamageTypeUnit(List<BaseUnit> units, ref List<BaseUnit> targets)
        {
            if (units.Count + targets.Count <= maxTargetsAmount)
            {
                targets.AddRange(units);
                return;
            }

            List<BaseUnit> oppositeDamageTypeUnits = new List<BaseUnit>();
            DamageType oppositeDamageType = GetOppositeDamageType();

            foreach (BaseUnit unit in units)
            {
                if (unit.DamageType != oppositeDamageType || targets.Contains(unit))
                    continue;

                oppositeDamageTypeUnits.Add(unit);
            }

            if (oppositeDamageTypeUnits.Count + targets.Count <= maxTargetsAmount)
            {
                targets.AddRange(oppositeDamageTypeUnits);
                return;
            }

            DetermineLeastHealthUnits(ref oppositeDamageTypeUnits, maxTargetsAmount - targets.Count);
            targets.AddRange(oppositeDamageTypeUnits);
        }

        protected void DetermineLeastHealthUnits(ref List<BaseUnit> units, int unitsAmount)
        {
            if (units.Count <= unitsAmount)
                return;

            float maxHealthAmount;
            BaseUnit maxHealthAmountUnit;

            do
            {
                maxHealthAmount = units[0].Health;
                maxHealthAmountUnit = units[0];

                foreach (BaseUnit unit in units)
                {
                    if (unit.Health <= maxHealthAmount)
                        continue;

                    maxHealthAmount = unit.Health;
                    maxHealthAmountUnit = unit;
                }

                units.Remove(maxHealthAmountUnit);
            } while (units.Count > unitsAmount);
        }
    }
}
