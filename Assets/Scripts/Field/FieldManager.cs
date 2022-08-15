using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldManager : MonoBehaviour
{
    public enum TileStatus
    {
        Free,
        Occupied,
        Standart
    }

    [SerializeField] private TileBase freeCellTile;
    [SerializeField] private TileBase occupiedCellTile;
    [SerializeField] private TileBase standartCellTile;

    private Tilemap fieldTilemap;
    private Camera mainCamera;

    private void OnEnable()
    {
        EventManager.OnDraggedUnitChangedPosition += ChangeCellTile;
    }

    private void OnDestroy()
    {
        EventManager.OnDraggedUnitChangedPosition += ChangeCellTile;
    }

    private void Start()
    {
        fieldTilemap = GetComponent<Tilemap>();
        mainCamera = Camera.main;
    }

    public void ChangeCellTile()
    {
        TileStatus tileStatus = TileStatus.Occupied;
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int cellPosition = fieldTilemap.WorldToCell(position);

        Debug.Log(cellPosition);

        if (fieldTilemap.GetTile(cellPosition) != standartCellTile)
            return;

        TileBase requiredCellTile = tileStatus switch
        {
            TileStatus.Free => freeCellTile,
            TileStatus.Occupied => occupiedCellTile,
            _ => standartCellTile,
        };

        fieldTilemap.SetTile(cellPosition, requiredCellTile);
    }
}
