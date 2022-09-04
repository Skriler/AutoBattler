using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Units;

namespace AutoBattler.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Shop Database", menuName = "Custom/ShopDatabase")]
    public class ShopDatabase : ScriptableObject
    {
        [Serializable]
        public struct ShopUnit
        {
            public BaseUnit prefab;
            public Sprite[] sprites;
            public UnitCharacteristics characteristics;
        }

        [SerializeField] private List<ShopUnit> shopUnits;

        public List<ShopUnit> GetUnits()
        {
            return shopUnits;
        }

        public List<ShopUnit> GetUnitsAtTavernTier(int playerTavernTier)
        {
            return shopUnits
                .Where(u => u.characteristics.TavernTier <= playerTavernTier)
                .ToList();
        }
    }
}
