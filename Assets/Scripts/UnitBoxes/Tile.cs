using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.UnitBoxes.Enums;

namespace AutoBattler.UnitBoxes
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Sprite standartCell;
        [SerializeField] private Sprite occupiedCell;
        [SerializeField] private Sprite freeCell;

        private SpriteRenderer spriteRenderer;

        private void Start()
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
