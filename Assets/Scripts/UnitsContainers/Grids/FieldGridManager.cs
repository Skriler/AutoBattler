using UnityEngine;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Grids
{
    public class FieldGridManager : GridManager
    {
        [SerializeField] private int activeWidth, activeHeight;
        [SerializeField] private Tile activeCellTile;

        public int ActiveWidth => activeWidth;
        public int ActiveHeight => activeHeight;

        public void SpawnEnemyUnits(BaseUnit[,] army, Transform enemyUnitsContainer)
        {
            int firstFreeTileIndex = 0;

            for (int i = 0; i < army.GetLength(0); ++i)
            {
                for (int j = 0; j < army.GetLength(1); ++j)
                {
                    if (army[i, j] == null)
                        continue;

                    Vector3 newUnitPosition = GetEmptyTilePositionForSecondArmy(firstFreeTileIndex);

                    BaseUnit newUnit = Instantiate(army[i, j], newUnitPosition, Quaternion.identity);
                    newUnit.transform.SetParent(enemyUnitsContainer);
                    newUnit.FlipOnX();
                    newUnit.ShowHealthBar();
                    army[i, j] = newUnit;

                    ++firstFreeTileIndex;
                }
            }
        }

        private Vector3 GetEmptyTilePositionForSecondArmy(int firstFreeTileIndex)
        {
            int currentTileIndex = 0;

            for (int i = tiles.GetLength(0) - 1; i >= 0; --i)
            {
                for (int j = 0; j < tiles.GetLength(1); ++j)
                {
                    if (currentTileIndex < firstFreeTileIndex)
                    {
                        ++currentTileIndex;
                        continue;
                    }

                    return tiles[i, j].transform.position;
                }
            }

            return new Vector3();
        }

        protected override bool IsFreeTile(Vector2Int index)
        {
            return IsActiveTile(index) && !unitsContainer.IsCellOccupied(index);
        }

        protected override Tile GetCurrentTile(Vector2Int index)
        {
            return index.x < activeWidth && index.y < activeHeight ? activeCellTile : cellTile;
        }

        private bool IsActiveTile(Vector2Int index)
        {
            return index.x < activeWidth && index.y < activeHeight;
        }
    }
}
