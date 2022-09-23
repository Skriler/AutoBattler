using UnityEngine;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Grids
{
    public class FieldGridManager : GridManager
    {
        [Header("Field spawn parameters")]
        [SerializeField] private int activeWidth;
        [SerializeField] private int activeHeight;
        [SerializeField] private Tile activeCellTile;

        public int ActiveWidth => activeWidth;
        public int ActiveHeight => activeHeight;

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
