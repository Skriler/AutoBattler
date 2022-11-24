using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.Members;

namespace AutoBattler.Managers.Battle
{
    public class SoloModeBattleManager : BattleManager
    {
        [Header("Parameters")]
        [SerializeField] private List<TavernTierLevelUpTurns> tavernTierLevelUpTurns;

        public override void StartBattle()
        {
            BaseUnit[,] playerEnemyUnits = GeneratePlayerEnemyArmy();
            EnterFightModeForMemberArmies(player, playerEnemyUnits);
        }

        public override void EndBattle()
        {
            ExitFightModeForArmies(player);
        }

        public override bool IsBattleEnded() => !IsBothMemberArmiesAlive(player);

        public override Dictionary<Member, bool> GetFightResults()
        {
            Dictionary<Member, bool> fightResults = new Dictionary<Member, bool>();

            fightResults.Add(player, IsMemberArmyAlive(player));

            return fightResults;
        }

        private BaseUnit[,] GeneratePlayerEnemyArmy()
        {
            int currentRound = GameManager.Instance.CurrentRound;

            int tavernTier = tavernTierLevelUpTurns
                .Where(obj => obj.levelUpTurn > currentRound)
                .First()
                .tavernTier;

            List<BaseUnit> units = GenerateUnits(tavernTier, currentRound);
            return CreateEnemyArmyFromUnits(units, tavernTier);
        }

        private List<BaseUnit> GenerateUnits(int tavernTier, int currentRound)
        {
            ShopUnitEntity keyUnit = shopDb.GetRandomShopUnitEntityAtTavernTier(tavernTier);

            List<BaseUnit> units = new List<BaseUnit>();
            units.Add(keyUnit.prefab);

            List<BaseUnit> sameRaceUnits = shopDb.GetUnitsWithRace(keyUnit.characteristics.Race, tavernTier);
            List<BaseUnit> sameSpecificationUnits = shopDb.GetUnitsWithSpecification(keyUnit.characteristics.Specification, tavernTier);

            units.AddRange(GenerateUnits(sameRaceUnits, tavernTier, currentRound));
            units.AddRange(GenerateUnits(sameSpecificationUnits, tavernTier, currentRound));

            return units;
        }

        private List<BaseUnit> GenerateUnits(List<BaseUnit> units, int tavernTier, int currentRound)
        {
            List<BaseUnit> generatedUnits = new List<BaseUnit>();

            int unitsAmount = tavernTier switch
            {
                1 => Random.Range(1, 3),
                2 => Random.Range(2, 4),
                3 => Random.Range(2, 5),
                4 => Random.Range(3, 6),
                5 => Random.Range(4, 7),
                _ => Random.Range(1, 7)
            };

            for (int i = 0; i < unitsAmount; ++i)
                generatedUnits.Add(units[Random.Range(0, units.Count)]);

            return generatedUnits;
        }

        private BaseUnit[,] CreateEnemyArmyFromUnits(List<BaseUnit> units, int tavernTier)
        {
            TavernTierOpenedTiles fieldTiles = CreateOpenedFieldTiles(
                player.Field.GetTavernTierOpenedTiles(tavernTier)
                );
            fieldTiles.tavernTier = tavernTier;

            Dictionary<Vector2Int, bool> tiles = new Dictionary<Vector2Int, bool>();

            fieldTiles.openedTiles.ForEach(t => tiles.Add(t, false));

            BaseUnit[,] enemyArmy = new BaseUnit[armyWidth, armyHeight];

            for (int i = 0; i < units.Count; ++i)
            {
                if (i >= fieldTiles.openedTiles.Count)
                    break;

                SetUnitOnBestPlace(enemyArmy, units[i], tiles);
            }

            return enemyArmy;
        }

        private TavernTierOpenedTiles CreateOpenedFieldTiles(List<TavernTierOpenedTiles> tavernTierOpenedTiles)
        {
            TavernTierOpenedTiles fieldTiles = new TavernTierOpenedTiles();

            fieldTiles.openedTiles = new List<Vector2Int>();

            foreach (TavernTierOpenedTiles tiles in tavernTierOpenedTiles)
                foreach (Vector2Int tile in tiles.openedTiles)
                    fieldTiles.openedTiles.Add(new Vector2Int(tile.x, tile.y));

            return fieldTiles;
        }

        private void SetUnitOnBestPlace(BaseUnit[,] units, BaseUnit unit, Dictionary<Vector2Int, bool> tiles)
        {
            int lineIndex = unit.Specification switch
            {
                UnitSpecification.Swordsman => 1,
                UnitSpecification.Archer => 0,
                UnitSpecification.Mage => 0,
                UnitSpecification.Assassin => 0,
                UnitSpecification.Knight => 1,
                UnitSpecification.Brute => 1,
                _ => 1
            };

            int index = IsFreeTileOnLine(tiles, lineIndex) ? lineIndex : GetAnotherLineIndex(lineIndex);

            PutUnitOnLine(units, unit, tiles, index);
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

        private void PutUnitOnLine(BaseUnit[,] units, BaseUnit unit, Dictionary<Vector2Int, bool> tiles, int lineIndex)
        {
            Dictionary<Vector2Int, bool> tilesOnLine = new Dictionary<Vector2Int, bool>();

            foreach (KeyValuePair<Vector2Int, bool> tile in tiles)
            {
                if (tile.Key.x != lineIndex || tile.Value == true)
                    continue;

                tilesOnLine.Add(tile.Key, tile.Value);
            }

            Vector2Int position = tilesOnLine.ElementAt(Random.Range(0, tilesOnLine.Count)).Key;

            units[position.x, position.y] = unit;
            tiles[position] = true;
        }
    }
}
