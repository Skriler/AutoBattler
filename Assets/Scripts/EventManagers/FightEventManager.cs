using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public class FightEventManager
    {
        public static UnityAction OnFightStarted;

        public static UnityAction OnFightEnded;

        public static void SendFightStarted()
            => OnFightStarted.Invoke();

        public static void SendFightEnded()
            => OnFightEnded.Invoke();
    }
}
