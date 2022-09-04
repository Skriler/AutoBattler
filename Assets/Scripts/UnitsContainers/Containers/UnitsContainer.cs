using UnityEngine;
using AutoBattler.Data.Units;

namespace AutoBattler.UnitsContainers.Containers
{
    public abstract class UnitsContainer : MonoBehaviour
    {
        public abstract void AddUnit(BaseUnit unit, Vector2Int index);
        public abstract void DeleteUnit(BaseUnit unit);
        public abstract void ChangeUnitPosition();
        public abstract bool IsCellOccupied(Vector2Int index);
        public abstract bool Contains(BaseUnit unit);
    }
}
