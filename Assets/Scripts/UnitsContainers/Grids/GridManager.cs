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
        [SerializeField] protected Vector2 spawnPosition;
        [SerializeField] protected int width, height;
        [SerializeField] protected Tile cellTile;

        protected GameObject tilesContainer;
        protected UnitsContainer unitsContainer;
        protected Tile[,] tiles;
        private Tile previousTile;

        private void OnEnable()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeTileSprite;
            UnitsEventManager.OnUnitEndDrag += ChangeUnitPosition;
            UnitsEventManager.OnUnitChangedPosition += DeleteUnit;
            UnitsEventManager.OnUnitSold += DeleteUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition -= ChangeTileSprite;
            UnitsEventManager.OnUnitEndDrag -= ChangeUnitPosition;
            UnitsEventManager.OnUnitChangedPosition -= DeleteUnit;
            UnitsEventManager.OnUnitSold -= DeleteUnit;
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
            position.x = (float)Math.Round(Convert.ToDouble(position.x));
            position.y = (float)Math.Round(Convert.ToDouble(position.y));
            position.z = 0;

            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < tiles.GetLength(1); ++j)
                {
                    if (tiles[i, j].transform.position != position)
                        continue;

                    index = new Vector2Int(i, j);
                    return tiles[i, j];
                }
            }

            index = new Vector2Int(-1, -1);
            return null;
        }

        protected void DeleteUnit(BaseUnit unit)
        {
            if (!unitsContainer.Contains(unit))
                return;

            unitsContainer.DeleteUnit(unit);
        }

        private void GenerateGrid()
        {
            tiles = new Tile[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Tile currentTile = GetCurrentTile(new Vector2Int(x, y));
                    Vector3 tileSpawnPosition = new Vector3(x + spawnPosition.x, y + spawnPosition.y);

                    Tile spawnedTile = Instantiate(currentTile, tileSpawnPosition, Quaternion.identity);
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
