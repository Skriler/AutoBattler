using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.SaveSystem;

namespace AutoBattler.UnitsContainers.Containers
{
    public class PlayerFieldContainer : FieldContainer, IDataPersistence
    {
        protected PlayerFieldGridManager playerFieldGridManager;

        public BuffContainer Buffs { get; private set; }

        protected override void Awake()
        {
            BuffsEventManager.OnBuffLevelIncreased += AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased += RemoveBuffEffect;

            base.Awake();

            playerFieldGridManager = GetComponent<PlayerFieldGridManager>();
            Buffs = transform.GetComponentInChildren<BuffContainer>();
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnBuffLevelIncreased -= AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased -= RemoveBuffEffect;
        }

        public override bool IsCellOccupied(Vector2Int index) => units[index.x, index.y] != null;

        public int GetOpenedCellsAmount() => playerFieldGridManager.GetOpenedCellsAmount();

        public override bool AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (base.AddUnit(unit, index))
            {
                UnitsEventManager.SendUnitAddedOnField(unit);
                return true;
            }

            return false;
        }

        public override bool RemoveUnit(BaseUnit unit)
        {
            if (base.RemoveUnit(unit))
            {
                UnitsEventManager.SendUnitRemovedFromField(unit);
                return true;
            }

            return false;
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

            foreach (UnitData unitData in data.field)
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
            UnitData unitData;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    unitData = new UnitData(units[i, j], i, j);
                    data.field.Add(unitData);
                }
            }
        }
    }
}
