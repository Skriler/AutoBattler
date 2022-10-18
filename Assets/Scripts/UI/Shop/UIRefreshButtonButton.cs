namespace AutoBattler.UI.Shop
{
    public class UIRefreshButtonButton : UIShopButton
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
