using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(
        fileName = "Buff Characteristics",
        menuName = "Custom/Characteristics/BuffCharacteristics"
        )]
    public class BuffCharacteristics : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private UnitCharacteristic targetCharacteristic;
        [SerializeField] private float bonus;
        [SerializeField] private int maxLevel;
        [SerializeField] private int unitsPerLevel;

        public string Title => title;
        public UnitCharacteristic TargetCharacteristic => targetCharacteristic;
        public float Bonus => bonus;
        public int MaxLevel => maxLevel;
        public int UnitsPerLevel => unitsPerLevel;
    }
}
