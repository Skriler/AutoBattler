using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;

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

        public List<ShopUnitEntity> GetUnitsAtTavernTier(int tavernTier)
        {
            return shopUnits
                .Where(u => u.characteristics.TavernTier <= tavernTier)
                .ToList();
        }

        public ShopUnitEntity GetShopUnitEntityByTitle(string title)
        {
            return shopUnits
                .Where(u => u.characteristics.Title == title)
                .FirstOrDefault();
        }

        public ShopUnitEntity GetRandomShopUnitEntityAtTavernTier(int tavernTier, int maxCost)
        {
            List<ShopUnitEntity> units = shopUnits
                .Where(u => u.characteristics.TavernTier == tavernTier)
                .Where(u => u.characteristics.Cost <= maxCost)
                .ToList();

            return units[Random.Range(0, units.Count)];
        }

        public List<BaseUnit> GetUnitsWithRace(UnitRace race, int maxTavernTier)
        {
            return shopUnits
                .Where(u => u.characteristics.TavernTier <= maxTavernTier)
                .Where(u => u.characteristics.Race == race)
                .Select(u => u.prefab)
                .ToList();
        }

        public List<BaseUnit> GetUnitsWithSpecification(UnitSpecification specification, int maxTavernTier)
        {
            return shopUnits
                .Where(u => u.characteristics.TavernTier <= maxTavernTier)
                .Where(u => u.characteristics.Specification == specification)
                .Select(u => u.prefab)
                .ToList();
        }

        public ShopUnitEntity GetUnitWithRace(UnitRace race, int maxTavernTier, int maxCost)
        {
            List<ShopUnitEntity> units = shopUnits
                .Where(u => u.characteristics.Race == race)
                .Where(u => u.characteristics.TavernTier <= maxTavernTier)
                .Where(u => u.characteristics.Cost <= maxCost)
                .ToList();

            if (units.Count == 0)
                return default(ShopUnitEntity);

            return units[Random.Range(0, units.Count)];
        }

        public ShopUnitEntity GetUnitWithSpecification(UnitSpecification specification, int maxTavernTier, int maxCost)
        {
            List<ShopUnitEntity> units = shopUnits
                .Where(u => u.characteristics.Specification == specification)
                .Where(u => u.characteristics.TavernTier <= maxTavernTier)
                .Where(u => u.characteristics.Cost <= maxCost)
                .ToList();

            if (units.Count == 0)
                return default(ShopUnitEntity);

            return units[Random.Range(0, units.Count)];
        }
    }
}
