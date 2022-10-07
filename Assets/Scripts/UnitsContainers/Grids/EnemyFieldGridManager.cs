using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Enums;

namespace AutoBattler.UnitsContainers.Grids
{
    public class EnemyFieldGridManager : GridManager
    {
        protected override TileStatus GetCurrentTileStatus(Vector2Int index) => TileStatus.Closed;

        public void SpawnUnits(BaseUnit[,] army, Transform unitsContainer)
        {
            Vector3 newUnitPosition;
            BaseUnit newUnit;

            for (int i = 0; i < army.GetLength(0); ++i)
            {
                for (int j = 0; j < army.GetLength(1); ++j)
                {
                    if (army[i, j] == null)
                        continue;

                    newUnitPosition = tiles[i, j].transform.position;

                    newUnit = Instantiate(army[i, j], newUnitPosition, Quaternion.identity);
                    newUnit.transform.SetParent(unitsContainer);
                    newUnit.EnterEnemyMode();
                    newUnit.ShowHealthBar();

                    army[i, j] = newUnit;
                }
            }
        }
    }
}
