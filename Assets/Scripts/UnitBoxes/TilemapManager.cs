using UnityEngine;
using UnityEngine.Tilemaps;
using AutoBattler.UnitBoxes.Structs;
using AutoBattler.UnitBoxes.Enums;

namespace AutoBattler.UnitBoxes
{
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField] private TileBase freeCellTile;
        [SerializeField] private TileBase occupiedCellTile;
        [SerializeField] private TileBase standartCellTile;

        private Tilemap unitBoxTilemap;
        private ChangedCellInfo lastChangedCellInfo;

        private void OnEnable()
        {
            EventManager.OnDraggedUnitChangedPosition += ChangeCellTile;
            EventManager.OnUnitEndDrag += ChangeUnitPosition;
        }

        private void OnDestroy()
        {
            EventManager.OnDraggedUnitChangedPosition -= ChangeCellTile;
            EventManager.OnUnitEndDrag -= ChangeUnitPosition;
        }

        void Start()
        {
            unitBoxTilemap = GetComponent<Tilemap>();
            lastChangedCellInfo = new ChangedCellInfo(new Vector3Int());
        }

        private void ChangeCellTile(Vector3 position)
        {
            StandardizeLastChangedCell();

            Vector3Int cellPosition = unitBoxTilemap.WorldToCell(position);

            if (unitBoxTilemap.GetTile(cellPosition) != standartCellTile)
                return;

            //TODO: Get status from player's army
            TileStatus tileStatus = TileStatus.Free;

            TileBase requiredCellTile = tileStatus switch
            {
                TileStatus.Free => freeCellTile,
                TileStatus.Occupied => occupiedCellTile,
                _ => standartCellTile,
            };

            unitBoxTilemap.SetTile(cellPosition, requiredCellTile);
            lastChangedCellInfo.SetPosition(cellPosition);
        }

        private void ChangeUnitPosition(Vector3 position)
        {
            StandardizeLastChangedCell();

            Vector3Int cellPosition = unitBoxTilemap.WorldToCell(position);

            if (unitBoxTilemap.GetTile(cellPosition) != standartCellTile)
                return;


        }

        private void StandardizeLastChangedCell()
        {
            if (lastChangedCellInfo.IsChanged)
            {
                unitBoxTilemap.SetTile(lastChangedCellInfo.Position, standartCellTile);
                lastChangedCellInfo.IsChanged = false;
            }
        }
    }
}
