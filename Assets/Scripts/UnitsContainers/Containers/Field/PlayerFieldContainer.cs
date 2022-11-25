using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.Buffs.Containers;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public class PlayerFieldContainer : MemberFieldContainer
    {
        public PlayerBuffContainer Buffs { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Buffs = transform.GetComponentInChildren<PlayerBuffContainer>();
        }

        public override MemberBuffContainer GetMemberBuffContainer() => Buffs;

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

        public override void LoadData(GameData data)
        {
            LoadDataFromMemberData(data.player);
        }

        public override void SaveData(GameData data)
        {
            SaveDataToMemberData(data.player);
        }
    }
}
