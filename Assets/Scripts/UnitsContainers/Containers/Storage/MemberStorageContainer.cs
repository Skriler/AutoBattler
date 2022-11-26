using UnityEngine;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;
using AutoBattler.Data.Members;
using System.Linq;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Managers;

namespace AutoBattler.UnitsContainers.Containers.Storage
{
    public abstract class MemberStorageContainer : StorageContainer, IDataPersistence
    {
        [SerializeField] protected Member owner;

        public abstract void LoadData(GameData data);

        public abstract void SaveData(GameData data);

        public void LoadDataFromMemberData(MemberData memberData)
        {
            ShopUnitsManager shopUnitsManager = ShopUnitsManager.Instance;
            ShopUnitEntity shopUnitEntity;

            foreach (UnitData unitData in memberData.storage)
            {
                shopUnitEntity = shopUnitsManager.GetShopUnitEntityByTitle(unitData.title);

                AddUnit(
                    shopUnitEntity,
                    new Vector2Int(unitData.x, unitData.y)
                    );

                units[unitData.x].SetUnitDataСharacteristics(unitData);
            }
        }

        public void SaveDataToMemberData(MemberData memberData)
        {
            memberData.storage.Clear();
            UnitData unitData;

            foreach (var (unit, i) in units.Select((unit, i) => (unit, i)))
            {
                if (unit == null)
                    continue;

                unitData = new UnitData(unit, i, 0);
                memberData.storage.Add(unitData);
            }
        }
    }
}
