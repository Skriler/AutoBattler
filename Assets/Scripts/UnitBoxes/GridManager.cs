using System;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.UnitBoxes.Enums;

namespace AutoBattler.UnitBoxes
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Vector2 spawnPosition;
        [SerializeField] private int width, height;
        [SerializeField] private Tile cellTile;

        private UnitBoxManager unitBoxManager;
        private Tile[,] tiles;
        private Tile previousTile;

        private void OnEnable()
        {
            EventManager.OnDraggedUnitChangedPosition += ChangeTileSprite;
            EventManager.OnUnitEndDrag += ChangeUnitPosition;
        }

        private void OnDestroy()
        {
            EventManager.OnDraggedUnitChangedPosition -= ChangeTileSprite;
            EventManager.OnUnitEndDrag -= ChangeUnitPosition;
        }

        private void Start()
        {
            unitBoxManager = GetComponent<UnitBoxManager>();
            GenerateGrid();
        }

        public int GetWidth() => width;
        public int GetHeight() => height;

        public Vector3 GetTilePositionByIndex(int x, int y)
        {
            return tiles[x, y].transform.position;
        }

        private void GenerateGrid()
        {
            tiles = new Tile[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Tile spawnedTile = Instantiate(
                        cellTile, 
                        new Vector3(x + spawnPosition.x, y + spawnPosition.y), 
                        Quaternion.identity
                        );

                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.transform.SetParent(gameObject.transform);
                    tiles[x, y] = spawnedTile;
                }
            }
        }

        private Tile GetTileAtPosition(Vector3 position, out int x, out int y)
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

                    x = i;
                    y = j;
                    return tiles[i, j];
                }
            }

            x = -1;
            y = -1;
            return null;
        }

        private void ChangeTileSprite(Vector3 position)
        {
            StandardizeLastChangedCell();

            Tile currentTile = GetTileAtPosition(position, out int x, out int y);

            if (currentTile == null)
                return;

            TileStatus tileStatus;

            if (unitBoxManager.IsCellOccupied(x, y))
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

        private void ChangeUnitPosition(Vector3 position)
        {
            StandardizeLastChangedCell();

            Tile currentTile = GetTileAtPosition(position, out int x, out int y);

            if (currentTile == null)
                return;

            if (unitBoxManager.IsCellOccupied(x, y))
                return;

            unitBoxManager.AddUnit();
        }
    }
}
