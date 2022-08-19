using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBoxManager : MonoBehaviour
{
    public abstract void AddUnit(int x, int y);
    public abstract void DeleteUnit();
    public abstract void ChangeUnitPosition();
    public abstract bool IsCellOccupied(int x, int y);
}
