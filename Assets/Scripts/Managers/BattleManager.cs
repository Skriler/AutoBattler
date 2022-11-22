using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Data.Members;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.Managers
{
    public class BattleManager : Manager<BattleManager>
    {
        [Header("Parameters")]
        [SerializeField] private const int firstTavernMaxRound = 3;
        [SerializeField] private const int secondTavernMaxRound = 6;
        [SerializeField] private const int thirdTavernMaxRound = 9;
        [SerializeField] private const int fourthTavernMaxRound = 12;

        private Player player;
        private List<Bot> bots;

        private int armyWidth;
        private int armyHeight;

        Dictionary<Member, Member> battlePairs;

        private ShopDatabase shopDb;

        private void Start()
        {
            shopDb = GameManager.Instance.ShopDb;
        }

        public bool IsMemberArmyAlive(Member member) => member.GetFieldContainer().IsAtLeastOneAliveUnit();

        public bool IsMemberEnemyArmyAlive(Member member) => member.GetEnemyFieldContainer().IsAtLeastOneAliveUnit();

        public bool IsBothMemberArmiesAlive(Member member) => IsMemberArmyAlive(member) && IsMemberEnemyArmyAlive(member);

        public void Setup(Player player, List<Bot> bots)
        {
            this.player = player;
            this.bots = new List<Bot>(bots);

            BaseUnit[,] playersArmy = player.Field.GetArmy();
            armyWidth = playersArmy.GetLength(0);
            armyHeight = playersArmy.GetLength(1);
        }

        public void StartSoloModeBattle()
        {
            BaseUnit[,] playerEnemyUnits = GeneratePlayerEnemyArmy();
            EnterFightModeForMemberArmies(player,  playerEnemyUnits);
        }

        public void EndSoloModeBattle()
        {
            ExitFightModeForArmies(player);
        }

        public void StartConfrontationModeBattle()
        {
            battlePairs = GenerateBattlePairs();

            if (battlePairs.Count == 0)
                return;

            BaseUnit[,] memberEnemyArmy;
            foreach (var battlePair in battlePairs)
            {
                memberEnemyArmy = GetCopyOfMemberArmy(battlePair.Key.GetFieldContainer());
                EnterFightModeForMemberArmies(battlePair.Value, memberEnemyArmy);

                memberEnemyArmy = GetCopyOfMemberArmy(battlePair.Value.GetFieldContainer());
                EnterFightModeForMemberArmies(battlePair.Key, memberEnemyArmy);
            }
        }

        public void EndConfrontationModeBattle()
        {
            foreach (var battlePair in battlePairs)
            {
                ExitFightModeForArmies(battlePair.Key);
                ExitFightModeForArmies(battlePair.Value);
            }

            battlePairs.Clear();
        }

        public bool IsConfrontationModeBattlesEnded()
        {
            Member member;
            foreach (var battlePair in battlePairs)
            {
                member = battlePair.Key;
                if (IsBothMemberArmiesAlive(member))
                    return false;

                member = battlePair.Value;
                if (IsBothMemberArmiesAlive(member))
                    return false;
            }

            return true;
        }

        public Dictionary<Member, bool> GetFightResults()
        {
            Dictionary<Member, bool> fightResults = new Dictionary<Member, bool>();

            Member member;
            foreach (var battlePair in battlePairs)
            {
                member = battlePair.Key;
                fightResults.Add(member, IsMemberArmyAlive(member));
                member = battlePair.Value;
                fightResults.Add(member, IsMemberArmyAlive(member));
            }

            return fightResults;
        }

        private void EnterFightModeForMemberArmies(Member member, BaseUnit[,] playerEnemyUnits)
        {
            member.EnemyField.SpawnUnits(playerEnemyUnits);

            BaseUnit[,] memberArmy = member.GetFieldContainer().GetArmy();
            BaseUnit[,] memberEnemyArmy = member.GetEnemyFieldContainer().GetArmy();

            for (int i = 0; i < armyWidth; ++i)
            {
                for (int j = 0; j < armyHeight; ++j)
                {
                    memberArmy[i, j]?.EnterFightMode(memberEnemyArmy);
                    memberEnemyArmy[i, j]?.EnterFightMode(memberArmy);
                }
            }
        }

        private void ExitFightModeForArmies(Member member)
        {
            BaseUnit[,] memberArmy = member.GetFieldContainer().GetArmy();
            BaseUnit[,] memberEnemyArmy = member.GetEnemyFieldContainer().GetArmy();

            for (int i = 0; i < armyWidth; ++i)
            {
                for (int j = 0; j < armyHeight; ++j)
                {
                    memberArmy[i, j]?.ExitFightMode();
                    memberEnemyArmy[i, j]?.ExitFightMode();
                }
            }
        }

        private BaseUnit[,] GeneratePlayerEnemyArmy()
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
            return CreateEnemyArmyFromUnits(units, tavernTier);
        }

        private List<BaseUnit> GenerateUnits(int tavernTier)
        {
            ShopUnitEntity keyUnit = shopDb.GetRandomShopUnitEntityAtTavernTier(tavernTier);

            List<BaseUnit> units = new List<BaseUnit>();
            units.Add(keyUnit.prefab);

            List<BaseUnit> sameRaceUnits = shopDb.GetUnitsWithRace(keyUnit.characteristics.Race, tavernTier);
            List<BaseUnit> sameSpecificationUnits = shopDb.GetUnitsWithSpecification(keyUnit.characteristics.Specification, tavernTier);

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
            TavernTierOpenedTiles fieldTiles = TransformIntoEnemyFieldTiles(
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

        private TavernTierOpenedTiles TransformIntoEnemyFieldTiles(List<TavernTierOpenedTiles> tavernTierOpenedTiles)
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

        private Dictionary<Member, Member> GenerateBattlePairs()
        {
            Dictionary<Member, Member> battlePairs = new Dictionary<Member, Member>();
            List<Member> battleMembers = new List<Member>(bots);

            int battlePairsAmount = (bots.Count + 1) / 2;

            if (battlePairsAmount <= 0)
                return battlePairs;

            int memberIndex = Random.Range(0, battleMembers.Count);

            battlePairs.Add(player, battleMembers[memberIndex]);
            battleMembers.RemoveAt(memberIndex);

            Member firstMember;
            Member secondMember;
            for (int i = 1; i < battlePairsAmount; ++i)
            {
                firstMember = battleMembers[Random.Range(0, battleMembers.Count)];
                battleMembers.Remove(firstMember);
                secondMember = battleMembers[Random.Range(0, battleMembers.Count)];
                battleMembers.Remove(secondMember);

                battlePairs.Add(firstMember, secondMember);
            }

            return battlePairs;
        }

        private BaseUnit[,] GetCopyOfMemberArmy(FieldContainer fieldContainer)
        {
            BaseUnit[,] copiedUnits = new BaseUnit[armyWidth, armyHeight];
            BaseUnit[,] memberArmy = fieldContainer.GetArmy();

            for (int i = 0; i < armyWidth; ++i)
            {
                for (int j = 0; j < armyHeight; ++j)
                {
                    if (memberArmy[i, j] == null)
                        continue;

                    copiedUnits[i, j] = shopDb.GetShopUnitEntityByTitle(memberArmy[i, j].Title).prefab;
                }
            }

            return copiedUnits;
        }
    }
}
