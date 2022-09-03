using UnityEngine;
using AutoBattler.UnitsContainers.Enums;

namespace AutoBattler.UnitsContainers.Grids
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] protected Sprite standartCell;
        [SerializeField] protected Sprite occupiedCell;
        [SerializeField] protected Sprite freeCell;

        private SpriteRenderer spriteRenderer;

        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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
    }
}
