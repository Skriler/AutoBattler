using System;
using UnityEngine;
using AutoBattler.UnitsContainers.Enums;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Grids
{
    public class TrashCanTile : Tile
    {
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

        private void ChangeTileStatus(Vector3 positon)
        {
            TileStatus currentStatus = TileStatus.Standart;

            if (IsPositionInTile(positon))
                currentStatus = TileStatus.Free;

            SetTileSprite(currentStatus);
        }

        private void SellUnit(BaseUnit unit, Vector3 positon)
        {
            SetTileSprite(TileStatus.Standart);

            if (!IsPositionInTile(positon))
                return;

            Destroy(unit.gameObject);
            UnitsEventManager.OnUnitSold(unit);
        }
    }
}
