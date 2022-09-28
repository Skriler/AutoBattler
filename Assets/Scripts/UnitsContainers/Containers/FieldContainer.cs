using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Containers
{
    public abstract class FieldContainer : UnitsContainer
    {
        protected GameObject unitsContainer;

        protected GridManager gridManager;
        protected BaseUnit[,] units;

        protected virtual void Start()
        {
            unitsContainer = transform.Find("Units").gameObject;

            gridManager = GetComponent<GridManager>();

            units = new BaseUnit[gridManager.Width, gridManager.Height];
        }

        public BaseUnit[,] GetArmy() => units;

        public int GetArmyWidth() => units.GetLength(0);

        public int GetArmyHeight() => units.GetLength(1);

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

        public override void ChangeUnitPosition()
        {

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
