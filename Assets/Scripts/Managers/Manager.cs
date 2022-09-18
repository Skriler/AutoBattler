using UnityEngine;

namespace AutoBattler.Managers
{
    public class Manager<T> : MonoBehaviour where T : Manager<T>
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            Instance = (T)this;
        }
    }
}
