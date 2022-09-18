using AutoBattler.UI.Tooltips;

namespace AutoBattler.Managers
{
    public abstract class UITooltipManager<T> : UITooltip where T : UITooltipManager<T>
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            Instance = (T)this;
        }
    }
}
