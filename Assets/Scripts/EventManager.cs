using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityAction<Vector3> OnDraggedUnitChangedPosition;

    public static UnityAction<Vector3> OnUnitEndDrag;

    public static void SendDraggedUnitChangedPosition(Vector3 position)
        => OnDraggedUnitChangedPosition.Invoke(position);

    public static void SendUnitEndDrag(Vector3 position)
        => OnUnitEndDrag.Invoke(position);
}
