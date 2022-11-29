using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;

namespace AutoBattler.Managers
{
    public class ShopUnitsManager : Manager<ShopUnitsManager>
    {
        [SerializeField] private ShopDatabase shopDb;

        public ShopDatabase ShopDb => shopDb;

        public List<ShopUnitEntity> GetUnits() => shopDb.shopUnits;

        public List<ShopUnitEntity> GetUnitsAtTavernTier(int tavernTier)
        {
            return shopDb.shopUnits
                .Where(u => u.characteristics.TavernTier <= tavernTier)
                .ToList();
        }

        public ShopUnitEntity GetShopUnitEntityByTitle(string title)
        {
            return shopDb.shopUnits
                .Where(u => u.characteristics.Title == title)
                .FirstOrDefault();
        }

        public List<BaseUnit> GetUnitsWithRace(UnitRace race, int maxTavernTier)
        {
            return shopDb.shopUnits
                .Where(u => u.characteristics.TavernTier <= maxTavernTier)
                .Where(u => u.characteristics.Race == race)
                .Select(u => u.prefab)
                .ToList();
        }

        public List<BaseUnit> GetUnitsWithSpecification(UnitSpecification specification, int maxTavernTier)
        {
            return shopDb.shopUnits
                .Where(u => u.characteristics.TavernTier <= maxTavernTier)
                .Where(u => u.characteristics.Specification == specification)
                .Select(u => u.prefab)
                .ToList();
        }

        public ShopUnitEntity GetRandomShopUnitEntityAtTavernTier(int tavernTier, int maxCost)
        {
            List<ShopUnitEntity> units = shopDb.shopUnits
                .Where(u => u.characteristics.TavernTier == tavernTier)
                .Where(u => u.characteristics.Cost <= maxCost)
                .ToList();

            if (units.Count == 0)
                return GetRandomShopUnitEntityAtTavernTierAndLower(tavernTier, maxCost);

            return units[Random.Range(0, units.Count)];
        }

        public ShopUnitEntity GetRandomShopUnitEntityAtTavernTierAndLower(int tavernTier, int maxCost)
        {
            List<ShopUnitEntity> units = shopDb.shopUnits
                .Where(u => u.characteristics.TavernTier <= tavernTier)
                .Where(u => u.characteristics.Cost <= maxCost)
                .ToList();

            return units[Random.Range(0, units.Count)];
        }

        public ShopUnitEntity GetUnitWithRace(UnitRace race, int maxTavernTier, int maxCost)
        {
            List<ShopUnitEntity> units = shopDb.shopUnits
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
            List<ShopUnitEntity> units = shopDb.shopUnits
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
