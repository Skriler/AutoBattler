using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Containers
{
    public class StorageContainer : UnitsContainer
    {
        private GameObject unitsContainer;
        private GridManager gridManager;
        private BaseUnit[] units;

        private void OnEnable()
        {
            UnitsEventManager.OnUnitBought += AddUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitBought -= AddUnit;
        }

        private void Start()
        {
            unitsContainer = transform.Find("Units").gameObject;
            gridManager = GetComponent<GridManager>();
            units = new BaseUnit[gridManager.Width];
        }

        public void AddUnit(ShopUnitEntity shopUnit)
        {
            if (IsFull())
            {
                Debug.Log("Storage is full");
                return;
            }

            int freeCellIndex = GetFreeCellIndex();

            if (freeCellIndex == -1)
            {
                Debug.Log("There is no free cell");
                return;
            }

            BaseUnit newUnit = Instantiate(shopUnit.prefab, unitsContainer.transform);
            newUnit.gameObject.name = shopUnit.characteristics.Title;
            newUnit.transform.position = 
                gridManager.GetTilePositionByIndex(freeCellIndex, 0);

            units[freeCellIndex] = newUnit;
        }

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            units[index.x] = unit;
            unit.transform.SetParent(unitsContainer.transform);
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

        public override void ChangeUnitPosition()
        {
           
        }

        public override bool IsCellOccupied(Vector2Int index)
        {
            return units[index.x] != null;
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
