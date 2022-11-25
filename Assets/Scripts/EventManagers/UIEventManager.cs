using UnityEngine.Events;

namespace AutoBattler.EventManagers
{
    public class UIEventManager
    {
        public static UnityAction OnScreenSizeChanged;

        public static void SendScreenSizeChanged()
            => OnScreenSizeChanged.Invoke();
    }
}
