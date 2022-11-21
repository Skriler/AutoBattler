using System.Linq;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Managers;
using AutoBattler.EventManagers;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.UnitsContainers.Containers.Storage
{
    public class PlayerStorageContainer : StorageContainer, IDataPersistence
    {
        protected override void Awake()
        {
            UnitsEventManager.OnUnitBought += AddUnit;

            base.Awake();
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitBought -= AddUnit;
        }

        public void LoadData(GameData data)
        {
            ShopDatabase shopDb = GameManager.Instance.ShopDb;
            ShopUnitEntity shopUnitEntity;

            foreach (UnitData unitData in data.player.storage)
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
            data.player.storage.Clear();
            UnitData unitData;

            foreach (var (unit, i) in units.Select((unit, i) => (unit, i)))
            {
                if (unit == null)
                    continue;

                unitData = new UnitData(unit, i, 0);
                data.player.storage.Add(unitData);
            }
        }
    }
}
