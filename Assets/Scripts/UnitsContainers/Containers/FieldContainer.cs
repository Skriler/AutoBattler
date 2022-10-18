using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Containers
{
    public abstract class FieldContainer : UnitsContainer
    {
        protected GridManager gridManager;
        protected GameObject unitsContainer;
        protected BaseUnit[,] units;

        protected virtual void Start()
        {
            gridManager = GetComponent<GridManager>();

            unitsContainer = transform.Find("Units").gameObject;

            units = new BaseUnit[gridManager.Width, gridManager.Height];
        }

        public BaseUnit[,] GetArmy() => units;

        public int GetArmyWidth() => units.GetLength(0);

        public int GetArmyHeight() => units.GetLength(1);

        public bool IsTileOnPosition(Vector3 position) => gridManager.IsTileOnPositon(position);

        public override bool AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return false;

            units[index.x, index.y] = unit;
            unit.transform.SetParent(unitsContainer.transform);
            unit.ShowHealthBar();
            return true;
        }

        public override bool RemoveUnit(BaseUnit unit)
        {
            if (!Contains(unit))
                return false;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j]?.Id != unit.Id)
                        continue;

                    units[i, j] = null;
                    unit.HideHealthBar();
                    return true;
                }
            }

            return false;
        }

        public override void ChangeUnitPosition(BaseUnit unit, Vector2Int index)
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j]?.Id != unit.Id)
                        continue;

                    units[i, j] = null;
                    units[index.x, index.y] = unit;
                    return;
                }
            }
        }

        public override bool Contains(BaseUnit unit)
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j]?.Id == unit.Id)
                        return true;
                }
            }

            return false;
        }

        public bool IsAtLeastOneAliveUnit()
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    if (units[i, j].IsAlive())
                        return true;
                }
            }

            return false;
        }

        public int GetUnitsAmount()
        {
            int unitsAmount = 0;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    unitsAmount++;
                }
            }

            return unitsAmount;
        }
    }
}
