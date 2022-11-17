using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.ScriptableObjects.Characteristics;

namespace AutoBattler.Data.ScriptableObjects.Databases
{
    [CreateAssetMenu(
        fileName = "Shop Database",
        menuName = "Custom/Database/ShopDatabase"
        )]
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

        public ShopUnitEntity GetShopUnitEntityByTitle(string title)
        {
            return shopUnits
                .Where(u => u.characteristics.Title == title)
                .FirstOrDefault();
        }
    }
}
