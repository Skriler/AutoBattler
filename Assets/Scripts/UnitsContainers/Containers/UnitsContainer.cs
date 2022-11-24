using UnityEngine;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Containers
{
    public abstract class UnitsContainer : MonoBehaviour
    {
        public bool CanPlaceUnits { get; set; } = true;

        public abstract void AddUnit(BaseUnit unit, Vector2Int index);

        public abstract void RemoveUnit(BaseUnit unit);

        public abstract void ChangeUnitPosition(BaseUnit unit, Vector2Int index);

        public abstract bool IsCellOccupied(Vector2Int index);

        public abstract bool Contains(BaseUnit unit);
    }
}
