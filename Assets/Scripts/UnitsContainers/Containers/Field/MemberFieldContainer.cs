using System.Collections.Generic;
using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Buffs.Containers;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Managers;
using AutoBattler.Data.Members;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.Enums;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public abstract class MemberFieldContainer : FieldContainer, IDataPersistence
    {
        [SerializeField] protected Member owner;

        protected MemberFieldGridManager memberFieldGridManager;

        protected override void Awake()
        {
            base.Awake();

            BuffsEventManager.OnBuffLevelIncreased += AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased += RemoveBuffEffect;

            memberFieldGridManager = GetComponent<MemberFieldGridManager>();
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnBuffLevelIncreased -= AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased -= RemoveBuffEffect;
        }

        public abstract void LoadData(GameData data);

        public abstract void SaveData(GameData data);

        public abstract MemberBuffContainer GetMemberBuffContainer();

        public override bool IsCellOccupied(Vector2Int index) => units[index.x, index.y] != null;

        public int GetOpenedCellsAmount() => memberFieldGridManager.GetOpenedCellsAmount();

        public List<TavernTierOpenedTiles> GetTavernTierOpenedTiles(int tavernTier) =>
            memberFieldGridManager.GetTavernTierOpenedTiles(tavernTier);

        public void AddBuffEffect(Buff buff)
        {
            if (!GetMemberBuffContainer().Contains(buff) || !buff.IsActive())
                return;

            ApplyCharacteristicBonus(buff.TargetCharacteristic, buff.Bonus);
        }

        public void RemoveBuffEffect(Buff buff)
        {
            if (!GetMemberBuffContainer().Contains(buff))
                return;

            float removedPointsAmount = -buff.Bonus;
            ApplyCharacteristicBonus(buff.TargetCharacteristic, removedPointsAmount);
        }

        public void ApplyCharacteristicBonus(UnitCharacteristic characteristic, float addedPointsAmount)
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    units[i, j]?.ApplyCharacteristicBonus(characteristic, addedPointsAmount);
                }
            }
        }

        public void LoadDataFromMemberData(MemberData memberData)
        {
            ShopDatabase shopDb = GameManager.Instance.ShopDb;
            ShopUnitEntity shopUnitEntity;

            foreach (UnitData unitData in memberData.field)
            {
                shopUnitEntity = shopDb.GetShopUnitEntityByTitle(unitData.title);

                AddUnit(
                    shopUnitEntity,
                    new Vector2Int(unitData.x, unitData.y)
                    );

                units[unitData.x, unitData.y].SetUnitDataСharacteristics(unitData);
            }
        }

        public void SaveDataToMemberData(MemberData memberData)
        {
            memberData.field.Clear();
            UnitData unitData;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    unitData = new UnitData(units[i, j], i, j);
                    memberData.field.Add(unitData);
                }
            }
        }
    }
}
