using System;
using UnityEngine;
using AutoBattler.UnitsContainers.Enums;

namespace AutoBattler.UnitsContainers.Grids
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] protected Sprite standartCell;
        [SerializeField] protected Sprite occupiedCell;
        [SerializeField] protected Sprite freeCell;

        protected SpriteRenderer spriteRenderer;
        protected Vector3 tilePosition;
        protected Vector3 tileSize;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            tileSize = spriteRenderer.bounds.size;
            tilePosition = transform.position;
        }

        public void SetTileSprite(TileStatus tileStatus)
        {
            Sprite requiredTileSprite = tileStatus switch
            {
                TileStatus.Free => freeCell,
                TileStatus.Occupied => occupiedCell,
                _ => standartCell,
            };

            spriteRenderer.sprite = requiredTileSprite;
        }

        public bool IsPositionInTile(Vector3 position)
        {
            if (position.x < tilePosition.x - tileSize.x / 2)
                return false;

            if (position.x > tilePosition.x + tileSize.x / 2)
                return false;

            if (position.y < tilePosition.y - tileSize.y / 2)
                return false;

            if (position.y > tilePosition.y + tileSize.y / 2)
                return false;

            return true;
        }
    }
}
