using UnityEngine.Events;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.Units;

namespace AutoBattler.EventManagers
{
    public class BuffsEventManager
    {
        public static UnityAction<Buff> OnBuffLevelIncreased;

        public static UnityAction<Buff> OnBuffLevelDecreased;

        public static UnityAction<BaseUnit> OnAppliedBuffsForUnit;

        public static UnityAction<BaseUnit> OnRemovedBuffsFromUnit;

        public static void SendBuffLevelIncreased(Buff buff)
            => OnBuffLevelIncreased.Invoke(buff);

        public static void SendBuffLevelDecreased(Buff buff)
            => OnBuffLevelDecreased.Invoke(buff);

        public static void SendAppliedBuffsForUnit(BaseUnit unit)
            => OnAppliedBuffsForUnit.Invoke(unit);

        public static void SendRemovedBuffsFromUnit(BaseUnit unit)
            => OnRemovedBuffsFromUnit.Invoke(unit);
    }
}
