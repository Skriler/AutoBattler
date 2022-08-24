using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using AutoBattler.UnitBoxes.Enums;

namespace AutoBattler.UnitBoxes
{
    public class StorageManager : UnitBoxManager
    {
        private GridManager gridManager;
        private BaseUnit[] units;

        private void Start()
        {
            gridManager = GetComponent<GridManager>();
            units = new BaseUnit[gridManager.GetWidth()];
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
            newUnit.transform.position = 
                gridManager.GetTilePositionByIndex(freeCellIndex, 0);

            units[freeCellIndex] = newUnit;
        }

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            units[index.x] = unit;
        }

        public override void DeleteUnit(BaseUnit unit)
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i]?.Id != unit.Id)
                    continue;

                units[i] = null;
                return;
            }
        }

        public override void ChangeUnitPosition()
        {
           
        }

        public override bool IsCellOccupied(Vector2Int index)
        {
            return units[index.x] != null;
        }

        public override bool Contains(BaseUnit unit)
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i]?.Id == unit.Id)
                    return true;
            }

            return false;
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
    }
}
