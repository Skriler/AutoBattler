using AutoBattler.UI.PlayerInfo;
using AutoBattler.UI.Tooltips;

namespace AutoBattler.UI.Shop
{
    public class UILevelUpButton : UIBaseObject
    {
        public int LevelUpCost { get; private set; }

        private string startDescription;

        protected override void Start()
        {
            base.Start();

            startDescription = Description;
        }

        public void UpdateDescription(int levelUpCost)
        {
            LevelUpCost = levelUpCost;
            Description = startDescription + LevelUpCost;

            UIBaseObjectTooltip.Instance.Setup(this);
        }
    }
}
