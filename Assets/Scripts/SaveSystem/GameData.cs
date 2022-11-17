using System;
using System.Collections.Generic;

namespace AutoBattler.SaveSystem
{
    [Serializable]
    public class GameData
    {
        public int currentRound;
        public int health;
        public int gold;
        public int tavernTier;
        public int levelUpTavernTierCost;

        public List<UnitData> storage;
        public List<UnitData> field;
        public List<BuffData> buffs;

        public GameData()
        {
            storage = new List<UnitData>();
            field = new List<UnitData>();
            buffs = new List<BuffData>();
        }
    }
}
