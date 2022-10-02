using System.Collections.Generic;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.ScriptableObjects.Databases;

namespace AutoBattler.Managers
{
    public class BattleManager
    {
        private PlayerFieldContainer playerFieldContainer;
        private EnemyFieldContainer enemyFieldContainer;
        private ShopDatabase shopDb;

        private BaseUnit[,] playerArmy;
        private BaseUnit[,] enemyArmy;

        private int ArmyWidth;
        private int ArmyHeight;

        public BattleManager(PlayerFieldContainer playerFieldContainer, EnemyFieldContainer enemyFieldContainer, ShopDatabase shopDb)
        {
            this.playerFieldContainer = playerFieldContainer;
            this.enemyFieldContainer = enemyFieldContainer;
            this.shopDb = shopDb;

            playerArmy = playerFieldContainer.GetArmy();
            enemyArmy = enemyFieldContainer.GetArmy();

            ArmyWidth = playerArmy.GetLength(0);
            ArmyHeight = enemyArmy.GetLength(1);

            GenerateSecondArmy();
            enemyFieldContainer.SpawnUnits(enemyArmy);
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
            List<ShopUnitEntity> shopUnits = shopDb.GetUnits();
            int shopUnitsAmount = shopUnits.Count;
            int unitsAmount = playerFieldContainer.GetUnitsAmount();

            for (int i = 0; i < ArmyWidth; ++i)
            {
                for (int j = 0; j < ArmyHeight; ++j)
                {
                    if (i * ArmyHeight + j >= unitsAmount)
                        continue;

                    enemyArmy[i, j] = shopUnits[UnityEngine.Random.Range(0, shopUnitsAmount)].prefab;
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
