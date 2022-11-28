using System;
using System.Collections.Generic;

namespace AutoBattler.SaveSystem.Data
{
    [Serializable]
    public class MemberData
    {
        public string id;
        public int health;
        public int gold;
        public int tavernTier;
        public int levelUpTavernTierCost;
        public int goldenCup;

        public List<UnitData> storage;
        public List<UnitData> field;
        public List<BuffData> buffs;

        public MemberData()
        {
            storage = new List<UnitData>();
            field = new List<UnitData>();
            buffs = new List<BuffData>();
        }
    }
}
