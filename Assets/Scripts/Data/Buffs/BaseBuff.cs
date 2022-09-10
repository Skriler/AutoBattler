using UnityEngine;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Databases;

namespace AutoBattler.Data.Buffs
{
    public abstract class BaseBuff
    {
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

        public bool IsActive { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public int CurrentUnitsAmount { get; protected set; }

        public void AddUnitsAmount()
        {
            ++CurrentUnitsAmount;

            if (CurrentLevel == MaxLevel)
                return;

            if (CurrentUnitsAmount < unitPerBuffLevel)
                return;

            ++CurrentLevel;
            CurrentUnitsAmount = 0;
        }
    }
}