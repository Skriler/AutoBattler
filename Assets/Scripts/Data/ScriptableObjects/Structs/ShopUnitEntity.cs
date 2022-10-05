using System;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.Data.ScriptableObjects.Characteristics;

namespace AutoBattler.Data.ScriptableObjects.Structs
{
    [Serializable]
    public struct ShopUnitEntity
    {
        [Header("Unit Parameters")]
        public BaseUnit prefab;
        public UnitCharacteristics characteristics;

        [Header("Shop Parameters")]
        public Sprite[] sprites;
        public float swapSpeed;
        public bool isFlipOnX;
    }
}
