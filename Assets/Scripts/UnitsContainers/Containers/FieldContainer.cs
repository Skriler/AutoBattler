using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Units;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.UnitsContainers.Containers
{
    public abstract class FieldContainer : UnitsContainer
    {
        protected GridManager gridManager;
        protected GameObject unitsContainer;
        protected BaseUnit[,] units;

        protected virtual void Awake()
        {
            unitsContainer = transform.Find("Units").gameObject;
            gridManager = GetComponent<GridManager>();
            units = new BaseUnit[gridManager.Width, gridManager.Height];
        }

        public BaseUnit[,] GetArmy() => units;

        public int GetArmyWidth() => units.GetLength(0);

        public int GetArmyHeight() => units.GetLength(1);

        public bool IsTileOnPosition(Vector3 position) => gridManager.IsTileOnPositon(position);

        public void AddUnit(ShopUnitEntity shopUnit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            BaseUnit newUnit = Instantiate(shopUnit.prefab);
            newUnit.gameObject.name = shopUnit.characteristics.Title;
            newUnit.transform.position = gridManager.GetTilePositionByIndex(index.x, index.y);

            AddUnit(newUnit, index);
        }

        public override bool AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return false;

            unit.transform.SetParent(unitsContainer.transform);
            unit.ShowHealthBar();
            units[index.x, index.y] = unit;
            
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
