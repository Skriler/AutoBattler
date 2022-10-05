using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.Units
{
    public class King : BaseUnit
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

            Dictionary<Vector2Int, BaseUnit> aliveUnits = new Dictionary<Vector2Int, BaseUnit>();

            for (int i = 0; i < enemyUnits.GetLength(0); ++i)
            {
                for (int j = 0; j < enemyUnits.GetLength(1); ++j)
                {
                    if (!IsUnitAlive(enemyUnits[i, j]))
                        continue;

                    aliveUnits.Add(new Vector2Int(i, j), enemyUnits[i, j]);
                }
            }

            currentTargets.Clear();
            DetermineOptimalTarget(aliveUnits, ref currentTargets);
        }

        protected void DetermineOptimalTarget(Dictionary<Vector2Int, BaseUnit> units, ref List<BaseUnit> targets)
        {
            if (units.Count <= 1)
            {
                targets.AddRange(units.Values);
                return;
            }

            Dictionary<BaseUnit, int> unitsNeighborsAmount = new Dictionary<BaseUnit, int>();
            DetermineUnitsNeighborsAmount(ref unitsNeighborsAmount, units);

            BaseUnit unitWithMaxNeighbors = unitsNeighborsAmount.FirstOrDefault(
                u => u.Value == unitsNeighborsAmount.Values.Max()
                ).Key;

            Vector2Int unitCoords = units.FirstOrDefault(u => u.Value == unitWithMaxNeighbors).Key;

            DetermineCoordsNeighbors(ref targets, units, unitCoords);
        }

        protected void DetermineUnitsNeighborsAmount(ref Dictionary<BaseUnit, int> unitsNeighborsAmount, Dictionary<Vector2Int, BaseUnit> units)
        {
            int neighborsAmount;

            foreach (KeyValuePair<Vector2Int, BaseUnit> unit in units)
            {
                neighborsAmount = 0;

                for (int x = unit.Key.x - 1; x <= unit.Key.x + 1; ++x)
                {
                    for (int y = unit.Key.y - 1; y <= unit.Key.y + 1; ++y)
                    {
                        if (x == unit.Key.x && y == unit.Key.y)
                            continue;

                        if (units.ContainsKey(new Vector2Int(x, y)))
                            ++neighborsAmount;
                    }
                }

                unitsNeighborsAmount.Add(unit.Value, neighborsAmount);
            }
        }

        protected void DetermineCoordsNeighbors(ref List<BaseUnit> targets, Dictionary<Vector2Int, BaseUnit> units, Vector2Int coords)
        {
            for (int x = coords.x - 1; x <= coords.x + 1; ++x)
            {
                for (int y = coords.y - 1; y <= coords.y + 1; ++y)
                {
                    Vector2Int currentCoords = new Vector2Int(x, y);

                    if (!units.ContainsKey(currentCoords))
                        continue;

                    targets.Add(units.GetValueOrDefault(currentCoords));
                }
            }
        }
    }
}
