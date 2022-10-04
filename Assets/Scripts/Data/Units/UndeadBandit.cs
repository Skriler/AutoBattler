using System.Collections.Generic;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.Units
{
    public class UndeadBandit : BaseUnit
    {
        protected BaseUnit currentTarget = null;

        protected override bool HasTargetedEnemy() => currentTarget != null;

        protected override void FindTarget(BaseUnit[,] enemyUnits)
        {
            if (enemyUnits == null)
                return;

            if (IsEnemyMode)
                FindTargetOnEnemyMode();
            else
                FindTargetOnNormalMode();

            if (fintTarget != null)
                StopCoroutine(fintTarget);

            fintTarget = StartCoroutine(FindTargetCoroutine());
        }

        protected void FindTargetOnNormalMode()
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
                    currentTarget = DetermineOptimalTarget(aliveUnits);
                    return;
                }
            }
        }

        protected void FindTargetOnEnemyMode()
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

        protected bool IsUnitAlive(BaseUnit unit)
        {
            if (unit == null)
                return false;

            return unit.IsAlive();
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

        protected override void DealDamageToTargetedEnemy()
        {
            if (currentTarget == null)
                return;

            currentTarget.TakeDamage(AttackDamage, DamageType);

            CheckTargetedEnemy();
        }

        protected override void CheckTargetedEnemy()
        {
            if (currentTarget == null)
                return;

            if (!currentTarget.IsAlive())
                currentTarget = null;
        }
    }
}
