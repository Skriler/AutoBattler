using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Members;
using AutoBattler.Data.Units;
using AutoBattler.Data.ScriptableObjects.Databases;

namespace AutoBattler.Managers.Battle
{
    public abstract class BattleManager : MonoBehaviour
    {
        protected Player player;
        protected List<Bot> bots;

        protected int armyWidth;
        protected int armyHeight;

        protected ShopDatabase shopDb;

        protected virtual void Start()
        {
            shopDb = GameManager.Instance.ShopDb;
        }

        public abstract void StartBattle();

        public abstract void EndBattle();

        public abstract bool IsBattleEnded();

        public abstract Dictionary<Member, bool> GetFightResults();

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

        protected void EnterFightModeForMemberArmies(Member member, BaseUnit[,] playerEnemyUnits)
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

        protected void ExitFightModeForArmies(Member member)
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
    }
}
