using AutoBattler.UI.PlayerInfo;
using AutoBattler.UI.Tooltips;

namespace AutoBattler.UI.Shop
{
    public class UIRefreshButtonButton : UIBaseObject
    {
        public int RefreshCost { get; private set; }

        public void UpdateDescription(int refreshCost)
        {
            if (RefreshCost != 0)
                return;

            RefreshCost = refreshCost;
            Description += refreshCost;
        }
    }
}
