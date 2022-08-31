using System;
using UnityEngine;
using AutoBattler.UnitBoxes.Enums;
using AutoBattler.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitBoxes
{
    public class TrashCanTile : Tile
    {
        private Vector3 tilePosition;

        private void OnEnable()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeTileStatus;
            UnitsEventManager.OnUnitEndDrag += SellUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeTileStatus;
            UnitsEventManager.OnUnitEndDrag -= SellUnit;
        }

        protected override void Start()
        {
            base.Start();
            tilePosition = transform.position;
            tilePosition.x = (float)Math.Round(Convert.ToDouble(tilePosition.x));
            tilePosition.y = (float)Math.Round(Convert.ToDouble(tilePosition.y));
            tilePosition.z = 0;
        }

        private void ChangeTileStatus(Vector3 position)
        {
            TileStatus currentStatus = TileStatus.Standart;

            if (IsPositionInTile(position))
                currentStatus = TileStatus.Free;

            SetTileSprite(currentStatus);
        }

        private void SellUnit(BaseUnit unit, Vector3 position)
        {
            SetTileSprite(TileStatus.Standart);

            if (!IsPositionInTile(position))
                return;

            Destroy(unit.gameObject);
            UnitsEventManager.OnUnitSold(unit);
        }

        private bool IsPositionInTile(Vector3 position)
        {
            position.x = (float)Math.Round(Convert.ToDouble(position.x));
            position.y = (float)Math.Round(Convert.ToDouble(position.y));
            position.z = 0;

            return tilePosition == position;
        }
    }
}
