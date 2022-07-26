﻿using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Members;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Managers.Battle
{
    public class ConfrontationModeBattleManager : BattleManager
    {
        private Dictionary<Member, Member> battlePairs;

        public override void StartBattle()
        {
            battlePairs = GenerateBattlePairs();

            if (battlePairs.Count == 0)
                return;

            BaseUnit[,] memberEnemyArmy;
            foreach (var battlePair in battlePairs)
            {
                if (battlePair.Key is Player && PlayerSettings.IsMuteOtherFields)
                    memberEnemyArmy = GetCopyOfMemberArmy(battlePair.Key.GetFieldContainer(), true);
                else
                    memberEnemyArmy = GetCopyOfMemberArmy(battlePair.Key.GetFieldContainer());

                EnterFightModeForMemberArmies(battlePair.Value, memberEnemyArmy);

                memberEnemyArmy = GetCopyOfMemberArmy(battlePair.Value.GetFieldContainer());
                EnterFightModeForMemberArmies(battlePair.Key, memberEnemyArmy);
            }
        }

        public override void EndBattle()
        {
            foreach (var battlePair in battlePairs)
            {
                ExitFightModeForArmies(battlePair.Key);
                ExitFightModeForArmies(battlePair.Value);
            }

            battlePairs.Clear();
        }

        public override bool IsBattleEnded()
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


        public override Dictionary<Member, bool> GetFightResults()
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

        private BaseUnit[,] GetCopyOfMemberArmy(FieldContainer fieldContainer, bool isMuteUnits = false)
        {
            BaseUnit[,] copiedUnits = new BaseUnit[armyWidth, armyHeight];
            BaseUnit[,] memberArmy = fieldContainer.GetArmy();

            for (int i = 0; i < armyWidth; ++i)
            {
                for (int j = 0; j < armyHeight; ++j)
                {
                    if (memberArmy[i, j] == null)
                        continue;

                    copiedUnits[i, j] = ShopUnitsManager.Instance
                        .GetShopUnitEntityByTitle(memberArmy[i, j].Title)
                        .prefab;

                    if (isMuteUnits)
                        copiedUnits[i, j].IsAttackSoundMuted = true;
                }
            }

            return copiedUnits;
        }
    }
}
