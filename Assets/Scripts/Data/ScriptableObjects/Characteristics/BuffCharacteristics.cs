using System;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(fileName = "Buff Characteristics", menuName = "Custom/BuffCharacteristics")]
    public class BuffCharacteristics : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private BuffType type;
        [SerializeField] private UnitCharacteristic targetCharacteristic;
        [SerializeField] private float statsAmount;
        [SerializeField] private int maxLevel;
        [SerializeField] private int unitsPerLevel;

        public string Title => title;
        public BuffType Type => type;
        public UnitCharacteristic TargetCharacteristic => targetCharacteristic;
        public float StatsAmount => statsAmount;
        public int MaxLevel => maxLevel;
        public int UnitsPerLevel => unitsPerLevel;
    }
}
