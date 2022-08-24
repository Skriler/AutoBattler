using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBoxManager : MonoBehaviour
{
    public abstract void AddUnit(BaseUnit unit, Vector2Int index);
    public abstract void DeleteUnit(BaseUnit unit);
    public abstract void ChangeUnitPosition();
    public abstract bool IsCellOccupied(Vector2Int index);
    public abstract bool Contains(BaseUnit unit);
}
