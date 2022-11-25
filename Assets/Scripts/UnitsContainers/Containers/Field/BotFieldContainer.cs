using System.Linq;
using UnityEngine;
using AutoBattler.SaveSystem.Data;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;
using AutoBattler.Data.Buffs.Containers;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public class BotFieldContainer : MemberFieldContainer
    {
        public BotBuffContainer Buffs { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Buffs = transform.GetComponentInChildren<BotBuffContainer>();
            CanPlaceUnits = false;
        }

        public override MemberBuffContainer GetMemberBuffContainer() => Buffs;

        public bool IsEmpty() => GetUnitsAmount() == 0;

        public void AddUnit(BaseUnit unit)
        {
            Vector2Int index = FindBestPlaceForUnit(unit);

            if (index.x < 0 || index.y < 0)
                return;

            AddUnit(unit, index);
        }

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            unit.transform.position = gridManager.GetTilePositionByIndex(index);
            unit.transform.SetParent(unitsContainer.transform);
            unit.ShowHealthBar();
            units[index.x, index.y] = unit;

            BotsEventManager.SendUnitAddedOnField(unit, owner.Id);
        }

        public override void RemoveUnit(BaseUnit unit)
        {
            Vector2Int unitPosition = GetUnitPosition(unit);

            if (IsUnitOnPosition(unitPosition))
            {
                units[unitPosition.x, unitPosition.y] = null;
                unit?.HideHealthBar();
                BotsEventManager.SendUnitRemovedFromField(unit, owner.Id);
            }
        }

        private Vector2Int FindBestPlaceForUnit(BaseUnit unit)
        {
            Vector2Int index = new Vector2Int(-1, -1);

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] != null)
                        continue;

                    if (!memberFieldGridManager.IsFreeTile(new Vector2Int(i, j)))
                        continue;

                    index.Set(i, j);
                    return index;
                }
            }

            return index;
        }

        public override void LoadData(GameData data)
        {
            MemberData memberData = data.bots.Where(b => b.id == owner.Id).First();

            LoadDataFromMemberData(memberData);
        }

        public override void SaveData(GameData data)
        {
            MemberData memberData;
            if (data.bots.Exists(b => b.id == owner.Id))
            {
                memberData = data.bots.Where(b => b.id == owner.Id).First();
            }
            else
            {
                memberData = new MemberData();
                memberData.id = owner.Id;
                data.bots.Add(memberData);
            }

            SaveDataToMemberData(memberData);
        }
    }
}
