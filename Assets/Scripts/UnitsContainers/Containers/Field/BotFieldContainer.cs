using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.SaveSystem.Data;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;
using AutoBattler.Data.Buffs.Containers;
using AutoBattler.Data.Enums;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public class BotFieldContainer : MemberFieldContainer
    {
        public BaseUnit[,] Units => units;

        public BotBuffContainer Buffs { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Buffs = transform.GetComponentInChildren<BotBuffContainer>();
            CanPlaceUnits = false;
        }

        public override MemberBuffContainer GetMemberBuffContainer() => Buffs;

        public bool IsEmpty() => GetUnitsAmount() == 0;

        public int GetFreeCellsAmount() => GetFreeCells().Count;

        public bool IsFull()
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] != null)
                        continue;

                    if (!memberFieldGridManager.IsFreeTile(new Vector2Int(i, j)))
                        continue;

                    return false;
                }
            }

            return true;
        }

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
            int preferdLineIndex = unit.Specification switch
            {
                UnitSpecification.Swordsman => 1,
                UnitSpecification.Archer => 0,
                UnitSpecification.Mage => 0,
                UnitSpecification.Assassin => 0,
                UnitSpecification.Knight => 1,
                UnitSpecification.Brute => 1,
                _ => 1
            };

            List<Vector2Int> freeCells = GetFreeCells();
            List<Vector2Int> freeCellsOnLine = GetFreeCellsOnLine(freeCells, preferdLineIndex);

            if (freeCellsOnLine.Count > 0)
                freeCells = freeCellsOnLine;

            return freeCells.ElementAt(Random.Range(0, freeCells.Count));
        }

        private List<Vector2Int> GetFreeCells()
        {
            List<Vector2Int> freeCells = new List<Vector2Int>();

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] != null)
                        continue;

                    if (!memberFieldGridManager.IsFreeTile(new Vector2Int(i, j)))
                        continue;

                    freeCells.Add(new Vector2Int(i, j));
                }
            }

            return freeCells;
        }

        private List<Vector2Int> GetFreeCellsOnLine(List<Vector2Int> cells, int line)
        {
            List<Vector2Int> freeCellsOnLine = new List<Vector2Int>();

            foreach (Vector2Int cell in cells)
            {
                if (cell.x != line)
                    continue;

                freeCellsOnLine.Add(cell);
            }

            return freeCellsOnLine;
        }

        public BaseUnit GetFirstUnit()
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    return units[i, j];
                }
            }

            return null;
        }

        public BaseUnit GetWeakestUnit() => GetMostUnit(true);

        public BaseUnit GetStrongestUnit() => GetMostUnit(false);

        private BaseUnit GetMostUnit(bool isWeakestRequired)
        {
            BaseUnit mostUnit = GetFirstUnit();

            if (mostUnit == null)
                return mostUnit;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    if (isWeakestRequired && IsUnitWeaker(units[i, j], mostUnit))
                        mostUnit = units[i, j];
                    else if (!isWeakestRequired && IsUnitStronger(units[i, j], mostUnit))
                        mostUnit = units[i, j];
                }
            }

            return mostUnit;
        }

        private bool IsUnitWeaker(BaseUnit newUnit, BaseUnit oldUnit) => newUnit.MaxHealth < oldUnit.MaxHealth;

        private bool IsUnitStronger(BaseUnit newUnit, BaseUnit oldUnit) => newUnit.MaxHealth > oldUnit.MaxHealth;

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
