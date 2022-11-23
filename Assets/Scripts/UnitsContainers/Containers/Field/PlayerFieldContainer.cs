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
    public class PlayerFieldContainer : MemberFieldContainer
    {
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
