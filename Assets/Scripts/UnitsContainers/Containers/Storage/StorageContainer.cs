using System.Linq;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.UnitsContainers.Containers.Storage
{
    public abstract class StorageContainer : UnitsContainer
    {
        protected GameObject unitsContainer;
        protected GridManager gridManager;
        protected BaseUnit[] units;

        protected virtual void Awake()
        {
            unitsContainer = transform.Find("Units").gameObject;
            gridManager = GetComponent<GridManager>();
            units = new BaseUnit[gridManager.Width];
        }

        public bool IsTileOnPosition(Vector3 position) => gridManager.IsTileOnPositon(position);

        public override bool IsCellOccupied(Vector2Int index) => units[index.x] != null;

        public void AddUnit(ShopUnitEntity shopUnit)
        {
            if (IsFull())
                return;

            int freeCellIndex = GetFreeCellIndex();

            if (freeCellIndex == -1)
                return;

            AddUnit(
                shopUnit, 
                new Vector2Int(freeCellIndex, 0)
                );
        }

        public void AddUnit(ShopUnitEntity shopUnit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            BaseUnit newUnit = Instantiate(shopUnit.prefab);
            newUnit.gameObject.name = shopUnit.characteristics.Title;
            newUnit.transform.position = gridManager.GetTilePositionByIndex(index.x, index.y);

            AddUnit(newUnit, index);
        }

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            unit.transform.SetParent(unitsContainer.transform);
            units[index.x] = unit;
        }

        public override void RemoveUnit(BaseUnit unit)
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i]?.Id != unit.Id)
                    continue;

                units[i] = null;
                return;
            }
        }

        public override void ChangeUnitPosition(BaseUnit unit, Vector2Int index)
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i]?.Id != unit.Id)
                    continue;

                units[i] = null;
                units[index.x] = unit;
                return;
            }
        }

        public override bool Contains(BaseUnit unit)
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i]?.Id == unit.Id)
                    return true;
            }

            return false;
        }

        public bool IsFull()
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    return false;
            }

            return true;
        }

        private int GetFreeCellIndex()
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    return i;
            }

            return -1;
        }
    }
}
