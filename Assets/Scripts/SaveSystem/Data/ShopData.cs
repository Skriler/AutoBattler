using System;
using System.Collections.Generic;

namespace AutoBattler.SaveSystem.Data
{
    [Serializable]
    public class ShopData
    {
        public bool isFreezed;
        public List<ShopUnitData> units;

        public ShopData()
        {
            units = new List<ShopUnitData>();
        }
    }
}