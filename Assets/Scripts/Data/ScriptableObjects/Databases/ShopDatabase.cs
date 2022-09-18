using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler.Data.ScriptableObjects.Databases
{
    [CreateAssetMenu(fileName = "Shop Database", menuName = "Custom/ShopDatabase")]
    public class ShopDatabase : ScriptableObject
    {
        [SerializeField] private List<ShopUnitEntity> shopUnits;

        public List<ShopUnitEntity> GetUnits() => shopUnits;

        public List<ShopUnitEntity> GetUnitsAtTavernTier(int playerTavernTier)
        {
            return shopUnits
                .Where(u => u.characteristics.TavernTier <= playerTavernTier)
                .ToList();
        }
    }
}
