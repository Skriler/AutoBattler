using UnityEngine;

namespace AutoBattler.UnitBoxes.Structs
{
    public struct ChangedCellInfo
    {
        public Vector3Int Position { get; private set; }
        public bool IsChanged { get; set; }

        public ChangedCellInfo(Vector3Int position)
        {
            Position = position;
            IsChanged = false;
        }

        public void SetPosition(Vector3Int position)
        {
            Position = position;
            IsChanged = true;
        }
    }
}
