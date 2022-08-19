using UnityEngine;
using UnityEngine.Tilemaps;
using AutoBattler.UnitBoxes.Enums;

namespace AutoBattler.UnitBoxes
{
    public class FieldManager : UnitBoxManager
    {
        private GridManager gridManager;
        private BaseUnit[,] units;

        private void Start()
        {
            gridManager = GetComponent<GridManager>();
            units = new BaseUnit[gridManager.GetWidth(), gridManager.GetHeight()];
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
            return units[x, y] != null;
        }
    }
}
