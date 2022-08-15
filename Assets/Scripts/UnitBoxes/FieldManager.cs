using UnityEngine;
using UnityEngine.Tilemaps;
using AutoBattler.UnitBoxes.Structs;
using AutoBattler.UnitBoxes.Enums;

namespace AutoBattler.UnitBoxes
{
    public class FieldManager : UnitBoxManager
    {
        [SerializeField] private TileBase fieldCellTile;

        private Tilemap fieldTilemap;

        private void Start()
        {
            fieldTilemap = GetComponent<Tilemap>();
        }

        public override void AddUnit(ShopDatabase.ShopUnit shopUnit)
        {
            
        }

        public override void DeleteUnit()
        {

        }

        public override void ChangeUnitPosition()
        {

        }

        public override bool IsCellOccupied(Vector3 position)
        {
            bool isCellOccupied = false;

            return isCellOccupied;
        }
    }
}
