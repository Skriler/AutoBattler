using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public class SaveSystemEventManager
    {
        public static UnityAction OnDataLoaded;

        public static void SendDataLoaded()
            => OnDataLoaded.Invoke();
    }
}
