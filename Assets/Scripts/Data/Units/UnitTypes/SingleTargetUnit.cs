using AutoBattler.Data.Enums;
using System.Collections.Generic;

namespace AutoBattler.Data.Units
{
    public abstract class SingleTargetUnit : BaseUnit
    {
        protected BaseUnit currentTarget = null;

        protected abstract void FindTargetOnEnemyMode();

        protected abstract void FindTargetOnNormalMode();

        protected override bool HasTarget() => currentTarget != null;

        protected override void DealDamageToTarget()
        {
            if (!HasTarget())
                return;

            currentTarget.TakeDamage(AttackDamage, DamageType);
        }

        protected override void FindTarget(BaseUnit[,] enemyUnits)
        {
            if (enemyUnits == null)
                return;

            if (IsEnemyMode)
                FindTargetOnEnemyMode();
            else
                FindTargetOnNormalMode();
        }

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

            float leastHealthAmount = units[0].Health;
            BaseUnit leastHealthAmountUnit = units[0];

            foreach (BaseUnit unit in units)
            {
                if (unit.Health >= leastHealthAmount)
                    continue;

                leastHealthAmount = unit.Health;
                leastHealthAmountUnit = unit;
            }

            return leastHealthAmountUnit;
        }
    }
}
