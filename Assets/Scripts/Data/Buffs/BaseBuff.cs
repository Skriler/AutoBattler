using UnityEngine;
using AutoBattler.Data.Enums;
using AutoBattler.Data.Units;

namespace AutoBattler.Data.Buffs
{
    public abstract class BaseBuff
    {
        private static int minLevel = 0;
        private static int startUnitsAmount = 0;

        [SerializeField] protected string title;
        [SerializeField] protected int maxLevel;
        [SerializeField] protected int unitPerBuffLevel;
        [SerializeField] protected UnitCharacteristic targetCharacteristic;
        [SerializeField] protected float statsAmount;

        public string Title => title;
        public int MaxLevel => maxLevel;
        public int UnitPerBuffLevel => unitPerBuffLevel;
        public UnitCharacteristic TargetCharacteristic => targetCharacteristic;
        public float StatsAmount => statsAmount;

        public bool IsActive { get; protected set; } = false;
        public int CurrentLevel { get; protected set; } = minLevel;
        public int CurrentUnitsAmount { get; protected set; } = startUnitsAmount;

        public abstract bool IsUnitPassCheck(BaseUnit unit);

        public void ResetBuff()
        {
            CurrentUnitsAmount = 0;
            CurrentLevel = 0;
            IsActive = false;
        }

        public virtual void AddBuff(BaseUnit unit)
        {
            if (!IsUnitPassCheck(unit))
                return;

            ++CurrentUnitsAmount;

            if (CurrentLevel == MaxLevel)
                return;

            if (CurrentUnitsAmount % UnitPerBuffLevel == 0)
                ++CurrentLevel;

            if (CurrentLevel > minLevel)
                IsActive = true;
        }

        public virtual void RemoveBuff(BaseUnit unit)
        {
            if (!IsUnitPassCheck(unit))
                return;

            if (CurrentUnitsAmount == startUnitsAmount)
                return;

            if (CurrentUnitsAmount % UnitPerBuffLevel == 0)
                --CurrentLevel;

            --CurrentUnitsAmount;

            if (CurrentLevel == minLevel)
                IsActive = false;
        }
    }
}