using System.Collections.Generic;
using System.Linq;
using AutoBattler.Data.Units;
using AutoBattler.SaveSystem.Data;
using UnityEngine;

namespace AutoBattler.UnitsContainers.Containers.Storage
{ 
    public class BotStorageContainer : MemberStorageContainer
    {
        public bool IsEmpty() => GetUnitsAmount() == 0;

        protected override void Awake()
        {
            base.Awake();

            CanPlaceUnits = false;
        }

        public void AddUnit(BaseUnit unit)
        {
            List<Vector2Int> freeCells = GetFreeCells();

            if (freeCells.Count == 0)
                return;

            Vector2Int unitIndex = freeCells.ElementAt(Random.Range(0, freeCells.Count));
            AddUnit(unit, unitIndex);
        }

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            base.AddUnit(unit, index);

            units[index.x].transform.position = gridManager.GetTilePositionByIndex(index);
            units[index.x].Draggable.IsActive = false;
            units[index.x].IsAttackSoundMuted = PlayerSettings.IsMuteOtherFields;   
        }

        public int GetUnitsAmount()
        {
            int unitsAmount = 0;

            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    continue;

                ++unitsAmount;
            }

            return unitsAmount;
        }

        public BaseUnit GetFirstUnit()
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    continue;

                return units[i];
            }

            return null;
        }

        public BaseUnit GetWeakestUnit() => GetMostUnit(true);

        public BaseUnit GetStrongestUnit() => GetMostUnit(false);

        private List<Vector2Int> GetFreeCells()
        {
            List<Vector2Int> freeCells = new List<Vector2Int>();

            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] != null)
                    continue;

                if (!gridManager.IsFreeTile(new Vector2Int(i, 0)))
                    continue;

                freeCells.Add(new Vector2Int(i, 0));
            }

            return freeCells;
        }

        private BaseUnit GetMostUnit(bool isWeakestRequired)
        {
            BaseUnit mostUnit = GetFirstUnit();

            if (mostUnit == null)
                return mostUnit;

            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    continue;

                if (isWeakestRequired && IsUnitWeaker(units[i], mostUnit))
                    mostUnit = units[i];
                else if (!isWeakestRequired && IsUnitStronger(units[i], mostUnit))
                    mostUnit = units[i];
            }

            return mostUnit;
        }

        private bool IsUnitWeaker(BaseUnit newUnit, BaseUnit oldUnit) => newUnit.MaxHealth < oldUnit.MaxHealth;

        private bool IsUnitStronger(BaseUnit newUnit, BaseUnit oldUnit) => newUnit.MaxHealth > oldUnit.MaxHealth;

        public override void LoadData(GameData data)
        {
            if (data.bots.Count == 0)
                return;

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
