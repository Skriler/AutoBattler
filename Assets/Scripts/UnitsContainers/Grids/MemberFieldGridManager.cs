using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.UnitsContainers.Enums;
using System.Linq;

namespace AutoBattler.UnitsContainers.Grids
{
    public abstract class MemberFieldGridManager : GridManager
    {
        [Header("Field spawn parameters")]
        [SerializeField] protected TavernTierOpenedTiles[] openedTilesPerTavernTiersArray;

        protected int currentTavernTier = 1;

        public int GetOpenedCellsAmount()
        {
            int openedCellsAmount = 0;

            foreach (Tile tile in tiles)
                if (tile.Status == TileStatus.Opened)
                    ++openedCellsAmount;

            return openedCellsAmount;
        }

        public override bool IsFreeTile(Vector2Int index)
        {
            return tiles[index.x, index.y].Status == TileStatus.Opened &&
                !unitsContainer.IsCellOccupied(index);
        }

        public List<TavernTierOpenedTiles> GetTavernTierOpenedTiles(int tavernTier)
        {
            return openedTilesPerTavernTiersArray
                .Where(t => t.tavernTier <= tavernTier)
                .ToList();
        }

        protected override TileStatus GetCurrentTileStatus(Vector2Int index)
        {
            return IsOpenedTile(index) ? TileStatus.Opened : TileStatus.Closed;
        }

        protected bool IsOpenedTile(Vector2Int index)
        {
            foreach (TavernTierOpenedTiles tavernTierOpenedTiles in openedTilesPerTavernTiersArray)
            {
                if (currentTavernTier < tavernTierOpenedTiles.tavernTier)
                    return false;

                foreach (Vector2Int tileCoords in tavernTierOpenedTiles.openedTiles)
                {
                    if (tileCoords == index)
                        return true;
                }
            }

            return false;
        }

        protected void OpenTiles(int tavernTier)
        {
            if (tavernTier == currentTavernTier)
                return;

            currentTavernTier = tavernTier;

            foreach (TavernTierOpenedTiles tavernTierOpenedTiles in openedTilesPerTavernTiersArray)
            {
                if (currentTavernTier < tavernTierOpenedTiles.tavernTier)
                    return;

                foreach (Vector2Int tileCoords in tavernTierOpenedTiles.openedTiles)
                    tiles[tileCoords.x, tileCoords.y].SetTileStatus(TileStatus.Opened);
            }
        }
    }
}
