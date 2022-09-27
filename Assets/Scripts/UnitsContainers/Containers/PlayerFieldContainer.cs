using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.Enums;

namespace AutoBattler.UnitsContainers.Containers
{
    public class PlayerFieldContainer : FieldContainer
    {
        private void OnEnable()
        {
            BuffsEventManager.OnBuffLevelIncreased += AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased += RemoveBuffEffect;
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnBuffLevelIncreased -= AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased -= RemoveBuffEffect;
        }

        public override bool AddUnit(BaseUnit unit, Vector2Int index)
        {
            bool result = base.AddUnit(unit, index);

            if (result)
                UnitsEventManager.OnUnitAddedOnField(unit);

            return result;
        }

        public override bool RemoveUnit(BaseUnit unit)
        {
            bool result = base.RemoveUnit(unit);

            if (result)
                UnitsEventManager.OnUnitRemovedFromField(unit);

            return result;
        }

        public void AddBuffEffect(Buff buff)
        {
            if (!buff.IsActive())
                return;

            Debug.Log(buff.Title + " added, level: " + buff.CurrentLevel);

            ApplyCharacteristicBonus(buff.TargetCharacteristic, buff.AddedPointsAmount);
        }

        public void RemoveBuffEffect(Buff buff)
        {
            Debug.Log(buff.Title + " removed, level: " + buff.CurrentLevel);

            float removedPointsAmount = -buff.AddedPointsAmount;
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
    }
}
