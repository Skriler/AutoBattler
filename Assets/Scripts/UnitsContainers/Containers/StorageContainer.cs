using System.Linq;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.SaveSystem;

namespace AutoBattler.UnitsContainers.Containers
{
    public class StorageContainer : UnitsContainer, IDataPersistence
    {
        private GameObject unitsContainer;
        private GridManager gridManager;
        private BaseUnit[] units;

        private void Awake()
        {
            UnitsEventManager.OnUnitBought += AddUnit;

            unitsContainer = transform.Find("Units").gameObject;
            gridManager = GetComponent<GridManager>();
            units = new BaseUnit[gridManager.Width];
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitBought -= AddUnit;
        }

        public bool IsTileOnPosition(Vector3 position) => gridManager.IsTileOnPositon(position);

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

        public override bool AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return false;

            unit.transform.SetParent(unitsContainer.transform);
            units[index.x] = unit;
            
            return true;
        }

        public override bool RemoveUnit(BaseUnit unit)
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i]?.Id != unit.Id)
                    continue;

                units[i] = null;
                return true;
            }
            return false;
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

        public void LoadData(GameData data)
        {
            ShopDatabase shopDb = GameManager.Instance.ShopDb;
            ShopUnitEntity shopUnitEntity;

            foreach (UnitData unitData in data.storage)
            {
                shopUnitEntity = shopDb.GetShopUnitEntityByTitle(unitData.title);

                AddUnit(
                    shopUnitEntity, 
                    new Vector2Int(unitData.x, unitData.y)
                    );

                units[unitData.x].SetUnitDataÑharacteristics(unitData);
            }
        }

        public void SaveData(GameData data)
        {
            UnitData unitData;

            foreach (var (unit, i) in units.Select((unit, i) => (unit, i)))
            {
                if (unit == null)
                    continue;

                unitData = new UnitData(unit, i, 0);
                data.storage.Add(unitData);
            }
        }
    }
}
