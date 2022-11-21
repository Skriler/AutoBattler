using AutoBattler.Data.Units;
using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Buffs;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public class BotFieldContainer : MemberFieldContainer
    {
        public void AddUnit(BaseUnit unit)
        {
            Vector2Int index = FindBestPlaceForUnit(unit);

            if (index.x < 0 || index.y < 0)
                return;

            AddUnit(unit, index);
        }

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            unit.transform.position = gridManager.GetTilePositionByIndex(index);
            unit.transform.SetParent(unitsContainer.transform);
            unit.ShowHealthBar();
            units[index.x, index.y] = unit;
        }

        public override void RemoveUnit(BaseUnit unit)
        {
            Vector2Int unitPosition = GetUnitPosition(unit);

            if (IsUnitOnPosition(unitPosition))
            {
                units[unitPosition.x, unitPosition.y] = null;
                unit?.HideHealthBar();
            }
        }

        private Vector2Int FindBestPlaceForUnit(BaseUnit unit)
        {
            Vector2Int index = new Vector2Int(-1, -1);

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] != null)
                        continue;

                    if (!memberFieldGridManager.IsFreeTile(new Vector2Int(i, j)))
                        continue;

                    index.Set(i, j);
                    return index;
                }
            }

            return index;
        }
    }
}
