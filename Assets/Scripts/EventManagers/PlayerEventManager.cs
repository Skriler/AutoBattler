using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public static class PlayerEventManager
    {
        public static UnityAction<int> OnGoldAmountChanged;

        public static UnityAction<int> OnHealthAmountChanged;

        public static UnityAction<int> OnTavernTierIncreased;

        public static void SendGoldAmountChanged(int goldAmount)
            => OnGoldAmountChanged.Invoke(goldAmount);

        public static void SendHealthAmountChanged(int healthAmount)
            => OnHealthAmountChanged.Invoke(healthAmount);

        public static void SendTavernTierIncreased(int tavernTier)
            => OnTavernTierIncreased.Invoke(tavernTier);
    }
}