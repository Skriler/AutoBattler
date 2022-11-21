using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.UnitsContainers.Enums;
using AutoBattler.Data.Enums;
using System.Linq;

namespace AutoBattler.Managers
{
    public class BattleManager
    {
        private PlayerFieldContainer playerFieldContainer;
        private EnemyFieldContainer enemyFieldContainer;

        private BaseUnit[,] playerArmy;
        private BaseUnit[,] enemyArmy;

        private int ArmyWidth;
        private int ArmyHeight;

        private ShopDatabase shopDb;

        public BattleManager(PlayerFieldContainer playerFieldContainer, EnemyFieldContainer enemyFieldContainer)
        {
            this.shopDb = GameManager.Instance.ShopDb;

            this.playerFieldContainer = playerFieldContainer;
            this.enemyFieldContainer = enemyFieldContainer;

            playerArmy = playerFieldContainer.GetArmy();
            enemyArmy = enemyFieldContainer.GetArmy();

            ArmyWidth = playerArmy.GetLength(0);
            ArmyHeight = enemyArmy.GetLength(1);

            GenerateSecondArmy();
            enemyFieldContainer.SpawnUnits();
        }

        public BaseUnit[,] GetSecondArmy() => enemyArmy;

        public void StartBattle()
        {
            for (int i = 0; i < ArmyWidth; ++i)
            {
                for (int j = 0; j < ArmyHeight; ++j)
                {
                    playerArmy[i, j]?.EnterFightMode(enemyArmy);
                    enemyArmy[i, j]?.EnterFightMode(playerArmy);
                }
            }
        }

        public void EndBattle()
        {
            for (int i = 0; i < ArmyWidth; ++i)
            {
                for (int j = 0; j < ArmyHeight; ++j)
                {
                    playerArmy[i, j]?.ExitFightMode();
                    enemyArmy[i, j]?.ExitFightMode();
                }
            }
        }

        private void GenerateSecondArmy()
        {
            int currentRound = GameManager.Instance.CurrentRound;

            int tavernTier = currentRound switch
            {
                <= 3 => 1,
                <= 6 => 2,
                <= 9 => 3,
                <= 12 => 4,
                _ => 5,
            };

            List<BaseUnit> units = GenerateUnits(tavernTier);
            ModifyEnemyArmy(units, tavernTier);

            //enemyArmy[0, 0] = shopUnits[8].prefab;
            //enemyArmy[0, 1] = null;
            //enemyArmy[0, 2] = shopUnits[8].prefab;
            //enemyArmy[0, 3] = shopUnits[11].prefab;
            //enemyArmy[0, 4] = shopUnits[11].prefab;
            //enemyArmy[1, 0] = null;
            //enemyArmy[1, 1] = shopUnits[11].prefab;
            //enemyArmy[1, 2] = shopUnits[8].prefab;
            //enemyArmy[1, 3] = shopUnits[7].prefab;
            //enemyArmy[1, 4] = shopUnits[11].prefab;
        }

        private List<BaseUnit> GenerateUnits(int tavernTier)
        {
            BaseUnit keyUnit = shopDb.GetRandomShopUnitEntityAtTavernTier(tavernTier).prefab;

            List<BaseUnit> units = new List<BaseUnit>();
            List<BaseUnit> sameRaceUnits = shopDb.GetUnitsWithRace(keyUnit.Race, tavernTier);
            List<BaseUnit> sameSpecificationUnits = shopDb.GetUnitsWithSpecification(keyUnit.Specification, tavernTier);

            units.AddRange(GenerateUnitsAmount(sameRaceUnits, tavernTier));
            units.AddRange(GenerateUnitsAmount(sameSpecificationUnits, tavernTier));

            return units;
        }

        private List<BaseUnit> GenerateUnitsAmount(List<BaseUnit> units, int tavernTier)
        {
            List<BaseUnit> generatedUnits = new List<BaseUnit>();

            int unitsAmount = tavernTier switch
            {
                1 => Random.Range(1, 2),
                2 => Random.Range(2, 3),
                3 => Random.Range(2, 3),
                4 => Random.Range(3, 4),
                5 => Random.Range(4, 5),
                _ => Random.Range(1, 5)
            };

            for (int i = 0; i < unitsAmount; ++i)
                generatedUnits.Add(units[Random.Range(0, units.Count)]);

            return generatedUnits;
        }

        private void ModifyEnemyArmy(List<BaseUnit> units, int tavernTier)
        {
            TavernTierOpenedTiles tavernTierOpenedTiles = 
                playerFieldContainer.GetTavernTierOpenedTiles(tavernTier);

            Dictionary<Vector2Int, bool> tiles = new Dictionary<Vector2Int, bool>();

            tavernTierOpenedTiles.openedTiles.ForEach(t => tiles.Add(t, false));

            for (int i = 0; i < units.Count; ++i)
            {
                if (i >= tavernTierOpenedTiles.openedTiles.Count)
                    break;

                SetUnitOnBestPlace(units[i], tiles);
            }
        }

        private void SetUnitOnBestPlace(BaseUnit unit, Dictionary<Vector2Int, bool> tiles)
        {
            int lineIndex = unit.Specification switch
            {
                UnitSpecification.Swordsman => 0,
                UnitSpecification.Archer => 0,
                UnitSpecification.Mage => 0,
                UnitSpecification.Assassin => 0,
                UnitSpecification.Knight => 0,
                UnitSpecification.Brute => 0,
            };

            TryPutUnitOnLine(unit, tiles, lineIndex);
        }

        private void TryPutUnitOnLine(BaseUnit unit, Dictionary<Vector2Int, bool> tiles, int lineIndex)
        {
            int index = IsFreeTileOnLine(tiles, lineIndex) ? lineIndex : GetAnotherLineIndex(lineIndex);

            PutUnitOnLine(unit, tiles, index);
        }

        private bool IsFreeTileOnLine(Dictionary<Vector2Int, bool> tiles, int lineIndex)
        {
            foreach (KeyValuePair<Vector2Int, bool> tile in tiles)
            {
                if (tile.Key.x != lineIndex || tile.Value == true)
                    continue;

                return true;
            }

            return false;
        }

        private int GetAnotherLineIndex(int lineIndex) => lineIndex == 1 ? 0 : 1;

        private void PutUnitOnLine(BaseUnit unit, Dictionary<Vector2Int, bool> tiles, int lineIndex)
        {
            Dictionary<Vector2Int, bool> tilesOnLine = new Dictionary<Vector2Int, bool>();

            foreach (KeyValuePair<Vector2Int, bool> tile in tiles)
            {
                if (tile.Key.x != lineIndex || tile.Value == true)
                    continue;

                tilesOnLine.Add(tile.Key, tile.Value);
            }

            Vector2Int position = tilesOnLine.ElementAt(Random.Range(0, tilesOnLine.Count - 1)).Key;

            enemyArmy[position.x, position.y] = unit;
            tiles[position] = true;
        }

        public bool IsFirstArmyAlive() => playerFieldContainer.IsAtLeastOneAliveUnit();

        public bool IsSecondArmyAlive()
        {
            for (int i = 0; i < enemyArmy.GetLength(0); ++i)
            {
                for (int j = 0; j < enemyArmy.GetLength(1); ++j)
                {
                    if (enemyArmy[i, j] == null)
                        continue;

                    if (enemyArmy[i, j].IsAlive())
                        return true;
                }
            }

            return false;
        }
    }
}
