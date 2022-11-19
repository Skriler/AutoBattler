using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.EventManagers;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Data.Buffs
{
    public abstract class Buff : MonoBehaviour
    {
        private static int MIN_LEVEL = 0;
        private static int START_UNITS_AMOUNT_ON_LEVEL = 0;

        [SerializeField] private BuffCharacteristics characteristics;

        public string Title { get; protected set; }
        public int MaxLevel { get; protected set; }
        public int UnitsPerLevel { get; protected set; }
        public UnitCharacteristic TargetCharacteristic { get; protected set; }
        public float Bonus { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public int UnitsAmountOnCurrentLevel { get; protected set; }

        private void Awake()
        {
            SetCharacteristics();
        }

        public abstract bool IsUnitPassCheck(BaseUnit unit);

        public bool IsActive() => CurrentLevel > MIN_LEVEL;

        private void SetCharacteristics()
        {
            Title = characteristics.Title;
            MaxLevel = characteristics.MaxLevel;
            UnitsPerLevel = characteristics.UnitsPerLevel;
            TargetCharacteristic = characteristics.TargetCharacteristic;
            Bonus = characteristics.Bonus;

            ResetProgress();
        }

        public void ResetProgress()
        {
            CurrentLevel = MIN_LEVEL;
            UnitsAmountOnCurrentLevel = START_UNITS_AMOUNT_ON_LEVEL;
        }

        public void AddBuff(BaseUnit unit)
        {
            if (!IsUnitPassCheck(unit))
                return;

            ++UnitsAmountOnCurrentLevel;

            if (CurrentLevel == MaxLevel)
                return;

            if (UnitsAmountOnCurrentLevel == UnitsPerLevel)
            {
                ++CurrentLevel;
                UnitsAmountOnCurrentLevel = START_UNITS_AMOUNT_ON_LEVEL;

                BuffsEventManager.SendBuffLevelIncreased(this);
            }
        }

        public void RemoveBuff(BaseUnit unit)
        {
            if (!IsUnitPassCheck(unit))
                return;

            if (UnitsAmountOnCurrentLevel == START_UNITS_AMOUNT_ON_LEVEL)
            {
                --CurrentLevel;
                UnitsAmountOnCurrentLevel = UnitsPerLevel;

                BuffsEventManager.SendBuffLevelDecreased(this);
            }

            --UnitsAmountOnCurrentLevel;
        }

        public void SetBuffData—haracteristics(BuffData buffData)
        {
            CurrentLevel = buffData.currentLevel;
            UnitsAmountOnCurrentLevel = buffData.unitsAmountOnCurrentLevel;
        }
    }
}