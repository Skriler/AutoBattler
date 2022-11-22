using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public class BotsEventManager
    {
        public static UnityAction<int, string> OnHealthAmountChanged;

        public static UnityAction<int, string> OnTavernTierIncreased;

        public static void SendHealthAmountChanged(int health, string id)
            => OnHealthAmountChanged.Invoke(health, id);

        public static void SendTavernTierIncreased(int tavernTier, string id)
            => OnTavernTierIncreased.Invoke(tavernTier, id);
    }
}
