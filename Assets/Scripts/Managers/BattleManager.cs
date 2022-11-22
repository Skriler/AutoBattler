using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.Enums;

namespace AutoBattler.Managers
{
    public class BattleManager : Manager<BattleManager>
    {
        [Header("Parameters")]
        [SerializeField] private const int firstTavernMaxRound = 3;
        [SerializeField] private const int secondTavernMaxRound = 6;
        [SerializeField] private const int thirdTavernMaxRound = 9;
        [SerializeField] private const int fourthTavernMaxRound = 12;

        private PlayerFieldContainer playerFieldContainer;
        private EnemyFieldContainer enemyFieldContainer;

        private BaseUnit[,] playerArmy;
        private BaseUnit[,] enemyArmy;

        private int ArmyWidth;
        private int ArmyHeight;

        private ShopDatabase shopDb;

        private void Start()
        {
            shopDb = GameManager.Instance.ShopDb;
        }

        public void Setup(PlayerFieldContainer playerFieldContainer, EnemyFieldContainer enemyFieldContainer)
        {
            this.playerFieldContainer = playerFieldContainer;
            this.enemyFieldContainer = enemyFieldContainer;

            playerArmy = playerFieldContainer.GetArmy();
            enemyArmy = enemyFieldContainer.GetArmy();

            ArmyWidth = playerArmy.GetLength(0);
            ArmyHeight = enemyArmy.GetLength(1);

            GenerateSecondArmy();
            enemyFieldContainer.SpawnUnits();
        }

        private void GenerateSecondArmy()
        {
            int currentRound = GameManager.Instance.CurrentRound;

            int tavernTier = currentRound switch
            {
                <= firstTavernMaxRound => 1,
                <= secondTavernMaxRound => 2,
                <= thirdTavernMaxRound => 3,
                <= fourthTavernMaxRound => 4,
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
            units.Add(keyUnit);

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
                1 => Random.Range(1, 3),
                2 => Random.Range(2, 4),
                3 => Random.Range(2, 4),
                4 => Random.Range(3, 5),
                5 => Random.Range(4, 6),
                _ => Random.Range(1, 6)
            };

            for (int i = 0; i < unitsAmount; ++i)
                generatedUnits.Add(units[Random.Range(0, units.Count)]);

            return generatedUnits;
        }

        private void ModifyEnemyArmy(List<BaseUnit> units, int tavernTier)
        {
            TavernTierOpenedTiles tavernTierOpenedTiles =
                playerFieldContainer.GetTavernTierOpenedTiles(tavernTier);
            TavernTierOpenedTiles enemyFieldTiles = TransformIntoEnemyFieldTiles(tavernTierOpenedTiles);

            Dictionary<Vector2Int, bool> tiles = new Dictionary<Vector2Int, bool>();

            tavernTierOpenedTiles.openedTiles.ForEach(t => tiles.Add(t, false));

            for (int i = 0; i < units.Count; ++i)
            {
                if (i >= tavernTierOpenedTiles.openedTiles.Count)
                    break;

                SetUnitOnBestPlace(units[i], tiles);
            }
        }

        private TavernTierOpenedTiles TransformIntoEnemyFieldTiles(TavernTierOpenedTiles tavernTierOpenedTiles)
        {
            TavernTierOpenedTiles enemyFieldTiles = new TavernTierOpenedTiles();

            enemyFieldTiles.tavernTier = tavernTierOpenedTiles.tavernTier;
            enemyFieldTiles.openedTiles = new List<Vector2Int>();

            int index;
            foreach (Vector2Int tile in tavernTierOpenedTiles.openedTiles)
            {
                index = tile.x == 0 ? 1 : 0;

                enemyFieldTiles.openedTiles.Add(new Vector2Int(index, tile.y));
            }

            return enemyFieldTiles;
        }

        private void SetUnitOnBestPlace(BaseUnit unit, Dictionary<Vector2Int, bool> tiles)
        {
            int lineIndex = unit.Specification switch
            {
                UnitSpecification.Swordsman => 0,
                UnitSpecification.Archer => 1,
                UnitSpecification.Mage => 1,
                UnitSpecification.Assassin => 1,
                UnitSpecification.Knight => 0,
                UnitSpecification.Brute => 0,
                _ => 0
            };

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

            Vector2Int position = tilesOnLine.ElementAt(Random.Range(0, tilesOnLine.Count)).Key;

            enemyArmy[position.x, position.y] = unit;
            tiles[position] = true;
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
