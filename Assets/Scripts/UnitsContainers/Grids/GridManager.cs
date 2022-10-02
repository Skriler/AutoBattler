using System;
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

        private void Awake()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeTileSprite;
            UnitsEventManager.OnUnitEndDrag += ChangeUnitPosition;
            UnitsEventManager.OnUnitChangedPosition += RemoveUnit;
            UnitsEventManager.OnUnitSold += RemoveUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition -= ChangeTileSprite;
            UnitsEventManager.OnUnitEndDrag -= ChangeUnitPosition;
            UnitsEventManager.OnUnitChangedPosition -= RemoveUnit;
            UnitsEventManager.OnUnitSold -= RemoveUnit;
        }

        private void Start()
        {
            tilesContainer = transform.Find("Tiles").gameObject;
            unitsContainer = GetComponent<UnitsContainer>();
            GenerateGrid();
        }

        public int Width => width;
        public int Height => height;

        public Vector3 GetTilePositionByIndex(int x, int y) => tiles[x, y].transform.position;

        protected virtual bool IsFreeTile(Vector2Int index) => !unitsContainer.IsCellOccupied(index);

        protected virtual Tile GetCurrentTile(Vector2Int index) => cellTile;

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
            Tile currentTile;
            Tile spawnedTile;
            Vector3 tileSpawnPosition;

            tiles = new Tile[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    currentTile = GetCurrentTile(new Vector2Int(x, y));

                    if (currentTile == null)
                        continue;

                    tileSpawnPosition = new Vector3(
                        x + spawnPosition.x + x * cellWidthSpacing, 
                        y + spawnPosition.y + y * cellHeightSpacing
                        );

                    spawnedTile = Instantiate(currentTile, tileSpawnPosition, Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.transform.SetParent(tilesContainer.transform);

                    tiles[x, y] = spawnedTile;
                }
            }
        }

        private void ChangeTileSprite(Vector3 position)
        {
            StandardizeLastChangedCell();

            Tile currentTile = GetTileAtPosition(position, out Vector2Int index);

            if (currentTile == null)
                return;

            TileStatus tileStatus;

            if (!IsFreeTile(index))
                tileStatus = TileStatus.Occupied;
            else
                tileStatus = TileStatus.Free;
                    
            currentTile.SetTileSprite(tileStatus);
            previousTile = currentTile;
        }

        private void StandardizeLastChangedCell()
        {
            if (previousTile == null)
                return;

            previousTile.SetTileSprite(TileStatus.Standart);
            previousTile = null;
        }

        private void ChangeUnitPosition(BaseUnit unit, Vector3 worldPosition)
        {
            StandardizeLastChangedCell();

            Tile currentTile = GetTileAtPosition(worldPosition, out Vector2Int index);

            if (currentTile == null)
                return;

            if (!IsFreeTile(index))
                return;

            UnitsEventManager.SendUnitChangedPosition(unit);

            unitsContainer.AddUnit(unit, index);
            unit.transform.position = currentTile.transform.position;
        }
    }
}
