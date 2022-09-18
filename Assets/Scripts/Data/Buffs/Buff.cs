using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.UI.Tooltips;
using AutoBattler.EventManagers;

namespace AutoBattler.Data.Buffs
{
    public class Buff : MonoBehaviour
    {
        private static int MIN_LEVEL = 0;
        private static int START_UNITS_AMOUNT_ON_LEVEL = 0;

        [SerializeField] private BuffCharacteristics characteristics;

        public string Title { get; protected set; }
        public BuffType Type { get; protected set; }
        public int MaxLevel { get; protected set; }
        public int UnitsPerLevel { get; protected set; }
        public UnitCharacteristic TargetCharacteristic { get; protected set; }
        public float StatsAmount { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public int UnitsAmountOnCurrentLevel { get; protected set; }

        private void Start()
        {
            SetÑharacteristics();
        }

        public void MouseExit() => UIBuffTooltip.Instance.Hide();

        public void MouseEnter()
        {
            UIBuffTooltip.Instance.Show();
            UIBuffTooltip.Instance.Setup(this);
        }

        private void SetÑharacteristics()
        {
            Title = characteristics.Title;
            Type = characteristics.Type;
            MaxLevel = characteristics.MaxLevel;
            UnitsPerLevel = characteristics.UnitsPerLevel;
            TargetCharacteristic = characteristics.TargetCharacteristic;
            StatsAmount = characteristics.StatsAmount;

            ResetProgress();
        }

        public void ResetProgress()
        {
            CurrentLevel = MIN_LEVEL;
            UnitsAmountOnCurrentLevel = START_UNITS_AMOUNT_ON_LEVEL;
        }

        public bool IsActive() => CurrentLevel > MIN_LEVEL;

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

                BuffsEventManager.OnBuffLevelIncreased(this);
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

                BuffsEventManager.OnBuffLevelDecreased(this);
            }

            --UnitsAmountOnCurrentLevel;
        }

        public bool IsUnitPassCheck(BaseUnit unit)
        {
            return unit.Race.ToString() == Type.ToString() ||
                unit.Specification.ToString() == Type.ToString();
        }
    }
}