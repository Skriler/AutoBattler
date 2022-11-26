using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.Units
{
    public abstract class FightUnit : BaseUnit
    {
        protected BaseUnit DetermineOptimalTarget(List<BaseUnit> units)
        {
            if (units == null)
                return null;

            if (units.Count == 1)
                return units[0];

            BaseUnit optimalTarget = DetermineLowHealthUnit(units);

            if (optimalTarget != null)
                return optimalTarget;

            optimalTarget = DetermineOppositeDamageTypeUnit(units);

            if (optimalTarget != null)
                return optimalTarget;

            return GetLeastHealthUnit(units);
        }

        protected BaseUnit DetermineLowHealthUnit(List<BaseUnit> units)
        {
            List<BaseUnit> lowHealthUnits = new List<BaseUnit>();

            foreach (BaseUnit unit in units)
            {
                if (!unit.HasLowHealth())
                    continue;

                lowHealthUnits.Add(unit);
            }

            return lowHealthUnits.Count switch
            {
                0 => null,
                1 => lowHealthUnits[0],
                _ => GetLeastHealthUnit(lowHealthUnits),
            };
        }

        protected BaseUnit DetermineOppositeDamageTypeUnit(List<BaseUnit> units)
        {
            if (units == null || units.Count == 0)
                return null;

            List<BaseUnit> oppositeDamageTypeUnits = new List<BaseUnit>();
            DamageType oppositeDamageType = GetOppositeDamageType();

            foreach (BaseUnit unit in units)
            {
                if (unit.DamageType != oppositeDamageType)
                    continue;

                oppositeDamageTypeUnits.Add(unit);
            }

            return oppositeDamageTypeUnits.Count switch
            {
                0 => null,
                1 => oppositeDamageTypeUnits[0],
                _ => GetLeastHealthUnit(oppositeDamageTypeUnits),
            };
        }

        protected BaseUnit GetLeastHealthUnit(List<BaseUnit> units)
        {
            if (units == null || units.Count == 0)
                return null;

            Dictionary<BaseUnit, float> leastHealthAmountUnits = new Dictionary<BaseUnit, float>();
            float leastHealthAmount = units[0].Health;

            foreach (BaseUnit unit in units)
            {
                if (unit.Health > leastHealthAmount)
                    continue;

                if (unit.Health < leastHealthAmount)
                {
                    leastHealthAmountUnits.Clear();
                    leastHealthAmount = unit.Health;
                    leastHealthAmountUnits.Add(unit, unit.Health);
                }
                else
                {
                    leastHealthAmountUnits.Add(unit, unit.Health);
                }
            }

            return leastHealthAmountUnits
                .ElementAt(Random.Range(0, leastHealthAmountUnits.Count))
                .Key;
        }
    }
}
