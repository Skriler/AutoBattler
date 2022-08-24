using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrashCan : MonoBehaviour
{
    private void OnEnable()
    {
        UnitsEventManager.OnUnitEndDrag += SellUnit;
    }

    private void OnDestroy()
    {
        UnitsEventManager.OnUnitEndDrag -= SellUnit;
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        
    }

    private void SellUnit(BaseUnit unit, Vector3 position)
    {
        
    }
}
