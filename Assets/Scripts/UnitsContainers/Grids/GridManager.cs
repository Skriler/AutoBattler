using UnityEngine;
using AutoBattler.UnitsContainers.Enums;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Grids
{
    public class GridManager : MonoBehaviour
    {
        [Header("Spawn parameters")]
        [SerializeField] protected Vector2 spawnPosition;
        [SerializeField] protected int width, height;
        [SerializeField] protected float cellWidthSpacing, cellHeightSpacing;

        [Header("Tile parameters")]
        [SerializeField] protected Tile cellTile;

        protected GameObject tilesContainer;
        protected UnitsContainer unitsContainer;
        protected Tile[,] tiles;

        private Tile previousTile;
        private TileStatus previousTileStatus;

        public int Width => width;
        public int Height => height;

        protected virtual void Awake()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeTileStatus;
            UnitsEventManager.OnUnitEndDrag += ChangeUnitPosition;
            UnitsEventManager.OnUnitChangedPosition += RemoveUnit;
            UnitsEventManager.OnUnitSold += RemoveUnit;

            tilesContainer = transform.Find("Tiles").gameObject;
            unitsContainer = GetComponent<UnitsContainer>();
            GenerateGrid();
        }

        protected virtual void OnDestroy()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition -= ChangeTileStatus;
            UnitsEventManager.OnUnitEndDrag -= ChangeUnitPosition;
            UnitsEventManager.OnUnitChangedPosition -= RemoveUnit;
            UnitsEventManager.OnUnitSold -= RemoveUnit;
        }

        public Vector3 GetTilePositionByIndex(int x, int y) => tiles[x, y].transform.position;

        protected virtual bool IsFreeTile(Vector2Int index) => !unitsContainer.IsCellOccupied(index);

        protected virtual TileStatus GetCurrentTileStatus(Vector2Int index) => TileStatus.Opened;

        public bool IsTileOnPositon(Vector3 position)
        {
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < tiles.GetLength(1); ++j)
                {
                    if (!tiles[i, j].IsPositionInTile(position))
                        continue;

                    return true;
                }
            }

            return false;
        }

        protected Tile GetTileAtPosition(Vector3 position, out Vector2Int index)
        {
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < tiles.GetLength(1); ++j)
                {
                    if (!tiles[i, j].IsPositionInTile(position))
                        continue;

                    index = new Vector2Int(i, j);
                    return tiles[i, j];
                }
            }

            index = new Vector2Int(-1, -1);
            return null;
        }

        protected void RemoveUnit(BaseUnit unit)
        {
            if (!unitsContainer.Contains(unit))
                return;

            unitsContainer.RemoveUnit(unit);
        }

        private void GenerateGrid()
        {
            TileStatus currentTileStatus;
            Tile spawnedTile;
            Vector3 tileSpawnPosition;

            tiles = new Tile[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    tileSpawnPosition = new Vector3(
                        x + spawnPosition.x + x * cellWidthSpacing, 
                        y + spawnPosition.y + y * cellHeightSpacing
                        );

                    spawnedTile = Instantiate(cellTile, tileSpawnPosition, Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.transform.SetParent(tilesContainer.transform);

                    currentTileStatus = GetCurrentTileStatus(new Vector2Int(x, y));
                    spawnedTile.SetTileStatus(currentTileStatus);

                    tiles[x, y] = spawnedTile;
                }
            }
        }

        private void ChangeTileStatus(Vector3 position)
        {
            StandardizePreviousChangedCell();

            Tile currentTile = GetTileAtPosition(position, out Vector2Int index);

            if (currentTile == null)
                return;

            TileStatus tileStatus;

            tileStatus = IsFreeTile(index) ? TileStatus.Free : TileStatus.Occupied;

            previousTileStatus = currentTile.Status;
            previousTile = currentTile;
            currentTile.SetTileStatus(tileStatus);
        }

        private void StandardizePreviousChangedCell()
        {
            if (previousTile == null)
                return;

            previousTile.SetTileStatus(previousTileStatus);
            previousTile = null;
        }

        private void ChangeUnitPosition(BaseUnit unit, Vector3 worldPosition)
        {
            StandardizePreviousChangedCell();

            Tile currentTile = GetTileAtPosition(worldPosition, out Vector2Int index);

            if (currentTile == null || !IsFreeTile(index))
                return;

            unit.transform.position = currentTile.transform.position;

            if (unitsContainer.Contains(unit))
            {
                unitsContainer.ChangeUnitPosition(unit, index);
            }
            else
            {
                UnitsEventManager.SendUnitChangedPosition(unit);
                unitsContainer.AddUnit(unit, index);
            }
        }
    }
}
