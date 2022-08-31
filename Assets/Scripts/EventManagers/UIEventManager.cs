using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public static class UIEventManager
    {
        public static UnityAction<int> OnGoldAmountChanged;

        public static UnityAction<int> OnHealthAmountChanged;

        public static void SendGoldAmountChanged(int goldAmount)
            => OnGoldAmountChanged.Invoke(goldAmount);

        public static void SendHealthAmountChanged(int healthAmount)
            => OnHealthAmountChanged.Invoke(healthAmount);
    }
}