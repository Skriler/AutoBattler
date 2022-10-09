using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.Enums;
using AutoBattler.UnitsContainers.Grids;

namespace AutoBattler.UnitsContainers.Containers
{
    public class PlayerFieldContainer : FieldContainer
    {
        protected PlayerFieldGridManager playerFieldGridManager;

        public BuffContainer Buffs { get; private set; }

        private void Awake()
        {
            BuffsEventManager.OnBuffLevelIncreased += AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased += RemoveBuffEffect;
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnBuffLevelIncreased -= AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased -= RemoveBuffEffect;
        }

        protected override void Start()
        {
            base.Start();

            playerFieldGridManager = GetComponent<PlayerFieldGridManager>();
            Buffs = transform.GetComponentInChildren<BuffContainer>();
        }

        public override bool IsCellOccupied(Vector2Int index) => units[index.x, index.y] != null;

        public int GetOpenedCellsAmount() => playerFieldGridManager.GetOpenedCellsAmount();

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
    }
}
