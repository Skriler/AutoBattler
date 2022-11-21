using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public class PlayerFieldContainer : MemberFieldContainer, IDataPersistence
    {
        protected override void Awake()
        {
            BuffsEventManager.OnBuffLevelIncreased += AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased += RemoveBuffEffect;

            base.Awake();
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnBuffLevelIncreased -= AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased -= RemoveBuffEffect;
        }

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            unit.transform.SetParent(unitsContainer.transform);
            unit.ShowHealthBar();
            units[index.x, index.y] = unit;

            UnitsEventManager.SendUnitAddedOnField(unit);
        }

        public override void RemoveUnit(BaseUnit unit)
        {
            Vector2Int unitPosition = GetUnitPosition(unit);

            if (IsUnitOnPosition(unitPosition))
            {
                units[unitPosition.x, unitPosition.y] = null;
                unit?.HideHealthBar();
                UnitsEventManager.SendUnitRemovedFromField(unit);
            }
        }

        public void AddBuffEffect(Buff buff)
        {
            if (!buff.IsActive())
                return;

            ApplyCharacteristicBonus(buff.TargetCharacteristic, buff.Bonus);
        }

        public void RemoveBuffEffect(Buff buff)
        {
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

        public void LoadData(GameData data)
        {
            ShopDatabase shopDb = GameManager.Instance.ShopDb;
            ShopUnitEntity shopUnitEntity;

            foreach (UnitData unitData in data.player.field)
            {
                shopUnitEntity = shopDb.GetShopUnitEntityByTitle(unitData.title);

                AddUnit(
                    shopUnitEntity,
                    new Vector2Int(unitData.x, unitData.y)
                    );

                units[unitData.x, unitData.y].SetUnitDataÑharacteristics(unitData);
            }
        }

        public void SaveData(GameData data)
        {
            data.player.field.Clear();
            UnitData unitData;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    unitData = new UnitData(units[i, j], i, j);
                    data.player.field.Add(unitData);
                }
            }
        }
    }
}
