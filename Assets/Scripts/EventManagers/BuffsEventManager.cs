using UnityEngine.Events;
using AutoBattler.Data.Buffs;

namespace AutoBattler.EventManagers
{
    public class BuffsEventManager
    {
        public static UnityAction<Buff> OnBuffLevelIncreased;

        public static UnityAction<Buff> OnBuffLevelDecreased;

        public static void SendBuffLevelIncreased(Buff buff)
            => OnBuffLevelIncreased.Invoke(buff);

        public static void SendBuffLevelDecreased(Buff buff)
            => OnBuffLevelDecreased.Invoke(buff);
    }
}
