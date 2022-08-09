using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StorageManager : MonoBehaviour
{
    [SerializeField] private GameObject storageGrid;
    [SerializeField] private int maxSize;

    private List<BaseUnit> units;
    private List<StorageCell> storageCells;

    void Start()
    {
        units = new List<BaseUnit>();
        storageCells = storageGrid.GetComponentsInChildren<StorageCell>().ToList();
    }

    public bool IsFull() => units.Count < maxSize;

    public void AddUnit(ShopDatabase.ShopUnit shopUnit)
    {
        if (IsFull())
        {
            Debug.Log("Storage is full");
            return;
        }

        StorageCell freeStorageCell = GetFreeCell();

        if (freeStorageCell == null)
        {
            Debug.Log("There is no free cell");
            return;
        }

        BaseUnit newUnit = Instantiate(shopUnit.prefab);
        newUnit.gameObject.name = shopUnit.title;
        newUnit.transform.position = freeStorageCell.transform.position;
        freeStorageCell.IsOccupied = true;

        units.Add(newUnit);
    }

    public void DeleteUnit()
    {
        
    }

    public StorageCell GetFreeCell()
    {
        StorageCell freeStorageCell = null;

        for (int i = 0; i < storageCells.Count; ++i)
        {
            if (storageCells[i].IsOccupied)
                continue;

            freeStorageCell = storageCells[i];
            break;
        }

        return freeStorageCell;
    }
}
