using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBoxManager : MonoBehaviour
{
    public abstract void AddUnit(ShopDatabase.ShopUnit shopUnit);
    public abstract void DeleteUnit();
    public abstract void ChangeUnitPosition();
    public abstract bool IsCellOccupied(Vector3 position);
}
