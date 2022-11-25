using AutoBattler.UI.Tooltips;

namespace AutoBattler.Managers
{
    public abstract class UITooltipManager<T> : UITooltip where T : UITooltipManager<T>
    {
        public static T Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Instance = (T)this;
        }
    }
}
