using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    //public static UnityEvent<FieldManager.TileStatus, Vector3> OnDraggedUnitChangedPosition;
    public static event UnityAction OnDraggedUnitChangedPosition;

    public static void SendDraggedUnitChangedPosition()
        => OnDraggedUnitChangedPosition.Invoke();
}
