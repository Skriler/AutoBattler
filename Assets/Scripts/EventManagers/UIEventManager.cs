using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public static class UIEventManager
    {
        public static UnityAction<int> OnGoldAmountChanged;

        public static UnityAction<int> OnHealthAmountChanged;

        public static UnityAction<int> OnTavernTierChanged;

        public static void SendGoldAmountChanged(int goldAmount)
            => OnGoldAmountChanged.Invoke(goldAmount);

        public static void SendHealthAmountChanged(int healthAmount)
            => OnHealthAmountChanged.Invoke(healthAmount);

        public static void SendTavernTierChanged(int tavernTier)
            => OnTavernTierChanged.Invoke(tavernTier);
    }
}