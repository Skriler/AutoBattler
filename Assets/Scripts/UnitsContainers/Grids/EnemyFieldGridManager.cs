using UnityEngine;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Grids
{
    public class EnemyFieldGridManager : GridManager
    {
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
                    newUnit.FlipOnX();
                    newUnit.ShowHealthBar();

                    army[i, j] = newUnit;
                }
            }
        }
    }
}
