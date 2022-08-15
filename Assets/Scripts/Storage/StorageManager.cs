using UnityEngine;
using UnityEngine.Tilemaps;

public class StorageManager : MonoBehaviour
{
    [SerializeField] public TileBase storageCellTile;

    private Tilemap storageTilemap;
    private BaseUnit[] units;

    private int maxSize = 0;

    private void Start()
    {
        storageTilemap = GetComponent<Tilemap>();

        CreateStorageCellTiles();

        units = new BaseUnit[maxSize];
    }

    private void CreateStorageCellTiles()
    {
        if (storageTilemap == null)
            return;

        BoundsInt bounds = storageTilemap.cellBounds;

        foreach (TileBase tile in storageTilemap.GetTilesBlock(bounds))
        {
            if (tile == storageCellTile)
                ++maxSize;
        }
    }

    public void AddUnit(ShopDatabase.ShopUnit shopUnit)
    {
        if (IsFull())
        {
            Debug.Log("Storage is full");
            return;
        }

        int freeCellIndex = GetFreeCellIndex();

        if (freeCellIndex == -1)
        {
            Debug.Log("There is no free cell");
            return;
        }

        BaseUnit newUnit = Instantiate(shopUnit.prefab);
        newUnit.gameObject.name = shopUnit.title;
        newUnit.transform.position = GetWorldCoordsByCellIndex(freeCellIndex);

        units[freeCellIndex] = newUnit;
    }

    public void DeleteUnit()
    {

    }

    private bool IsFull()
    {
        for (int i = 0; i < units.Length; ++i)
        {
            if (units[i] == null)
                return false;
        }

        return true;
    }

    private int GetFreeCellIndex()
    {
        for (int i = 0; i < units.Length; ++i)
        {
            if (units[i] == null)
                return i;
        }

        return -1;
    }

    private Vector3 GetWorldCoordsByCellIndex(int index)
    {
        Vector3Int cellCoords = storageTilemap.cellBounds.min;

        cellCoords.y += 1;
        cellCoords.x += index + 1;
        cellCoords.z = 0;

        Vector3 worldCoords = storageTilemap.CellToWorld(cellCoords);

        worldCoords.x += 0.5f;
        worldCoords.y += 0.5f;

        return worldCoords;
    }
}
