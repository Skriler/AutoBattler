using UnityEngine;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Grids
{
    public class PlayerFieldGridManager : GridManager
    {
        [Header("Field spawn parameters")]
        [SerializeField] private int activeWidth;
        [SerializeField] private int activeHeight;
        [SerializeField] private Tile activeCellTile;

        public int ActiveWidth => activeWidth;
        public int ActiveHeight => activeHeight;

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
