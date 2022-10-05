using UnityEngine;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Containers
{
    public abstract class UnitsContainer : MonoBehaviour
    {
        // Return true if added successfully
        public abstract bool AddUnit(BaseUnit unit, Vector2Int index);

        // Return true if removed successfully
        public abstract bool RemoveUnit(BaseUnit unit);

        public abstract void ChangeUnitPosition(BaseUnit unit, Vector2Int index);

        public abstract bool IsCellOccupied(Vector2Int index);

        public abstract bool Contains(BaseUnit unit);
    }
}
