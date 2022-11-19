using System;
using AutoBattler.Data.Buffs;

namespace AutoBattler.SaveSystem.Data
{
    [Serializable]
    public class BuffData
    {
        public string title;
        public int currentLevel;
        public int unitsAmountOnCurrentLevel;

        public BuffData(Buff buff)
        {
            title = buff.Title;
            currentLevel = buff.CurrentLevel;
            unitsAmountOnCurrentLevel = buff.UnitsAmountOnCurrentLevel;
        }
    }
}
