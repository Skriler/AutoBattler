using System;
using UnityEngine;
using AutoBattler.UnitsContainers.Enums;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Grids
{
    public class TrashCanTile : Tile
    {
        private void Awake()
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
            TileStatus currentStatus = TileStatus.Opened;

            if (IsPositionInTile(positon))
                currentStatus = TileStatus.Free;

            SetTileStatus(currentStatus);
        }

        private void SellUnit(BaseUnit unit, Vector3 positon)
        {
            SetTileStatus(TileStatus.Opened);

            if (!IsPositionInTile(positon))
                return;

            Destroy(unit.gameObject);
            UnitsEventManager.OnUnitSold(unit);
        }
    }
}
