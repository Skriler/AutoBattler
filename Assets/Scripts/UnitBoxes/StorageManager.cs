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

        public override void AddUnit(int x, int y)
        {
            
        }

        public override void DeleteUnit()
        {

        }

        public override void ChangeUnitPosition()
        {
           
        }

        public override bool IsCellOccupied(int x, int y)
        {
            return units[x] != null;
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
