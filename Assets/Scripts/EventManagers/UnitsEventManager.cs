using UnityEngine;
using UnityEngine.Events;
using AutoBattler.Units;

namespace AutoBattler.EventManagers
{
    public static class UnitsEventManager
    {
        public static UnityAction<Vector3> OnDraggedUnitChangedPosition;

        public static UnityAction<BaseUnit, Vector3> OnUnitEndDrag;

        public static UnityAction<BaseUnit> OnUnitChangedPosition;

        public static UnityAction<BaseUnit> OnUnitSold;

        public static void SendDraggedUnitChangedPosition(Vector3 position)
            => OnDraggedUnitChangedPosition.Invoke(position);

        public static void SendUnitEndDrag(BaseUnit unit, Vector3 position)
            => OnUnitEndDrag.Invoke(unit, position);

        public static void SendUnitChangedPosition(BaseUnit unit)
            => OnUnitChangedPosition.Invoke(unit);

        public static void SendUnitSold(BaseUnit unit)
            => OnUnitSold.Invoke(unit);
    }
}
