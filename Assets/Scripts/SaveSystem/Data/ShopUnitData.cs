using System;

namespace AutoBattler.SaveSystem.Data
{
    [Serializable]
    public class ShopUnitData
    {
        public string title;
        public int index;
        public bool isActive;

        public ShopUnitData(string title, int index, bool isActive)
        {
            this.title = title;
            this.index = index;
            this.isActive = isActive;
        }
    }
}