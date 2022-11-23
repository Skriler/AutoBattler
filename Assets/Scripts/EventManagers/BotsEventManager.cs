using AutoBattler.Data.Units;
using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public class BotsEventManager
    {
        public static UnityAction<int, string> OnHealthAmountChanged;

        public static UnityAction<int, string> OnTavernTierIncreased;

        public static UnityAction<BaseUnit, string> OnUnitAddedOnField;

        public static UnityAction<BaseUnit, string> OnUnitRemovedFromField;

        public static void SendHealthAmountChanged(int health, string id)
            => OnHealthAmountChanged.Invoke(health, id);

        public static void SendTavernTierIncreased(int tavernTier, string id)
            => OnTavernTierIncreased.Invoke(tavernTier, id);

        public static void SendUnitAddedOnField(BaseUnit unit, string id)
            => OnUnitAddedOnField.Invoke(unit, id);

        public static void SendUnitRemovedFromField(BaseUnit unit, string id)
            => OnUnitRemovedFromField.Invoke(unit, id);
    }
}
