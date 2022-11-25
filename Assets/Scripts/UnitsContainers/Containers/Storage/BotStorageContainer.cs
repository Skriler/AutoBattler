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

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            if (IsCellOccupied(index))
                return;

            base.AddUnit(unit, index);
            units[index.x].SetDraggableActive(false);
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

        public BaseUnit GetUnit()
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    continue;

                return units[i];
            }

            return null;
        }

        public BaseUnit GetWeakestUnit()
        {
            BaseUnit weakestUnit = null;

            for (int i = 0; i < units.Length; ++i)
            {
                //for (int j = 0; j < Field.GetLength(1); ++j)
                //{
                //    if (Field[i, j] == null)
                //        continue;

                //    if (!IsNewUnitBetter(Field[i, j], weakestUnit))
                //    {
                //        weakestUnit = Field[i, j];
                //        weakestUnitCoords.X = i;
                //        weakestUnitCoords.Y = j;
                //    }
                //}
            }

            return weakestUnit;
        }

        //private bool IsNewUnitBetter(Unit newUnit, Unit oldUnit)
        //{
        //    if (oldUnit == null)
        //        return true;

        //    int newUnitScore = 0;
        //    int oldUnitScore = 0;

        //    CheckCharacteristic(newUnit.StartHealth, oldUnit.StartHealth, ref newUnitScore, ref oldUnitScore);
        //    CheckCharacteristic(newUnit.SpecificationPart.Strength, oldUnit.SpecificationPart.Strength, ref newUnitScore, ref oldUnitScore);
        //    CheckCharacteristic(newUnit.SpecificationPart.AttackRange, oldUnit.SpecificationPart.AttackRange, ref newUnitScore, ref oldUnitScore);

        //    if (newUnitScore > oldUnitScore)
        //        return true;
        //    else
        //        return false;

        //}

        //private void CheckCharacteristic(int newUnitChar, int oldUnitChar, ref int newUnitScore, ref int oldUnitScore)
        //{
        //    if (newUnitChar >= oldUnitChar)
        //        ++newUnitScore;
        //    else
        //        ++oldUnitScore;
        //}

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
