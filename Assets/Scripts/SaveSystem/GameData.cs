using System;
using System.Collections.Generic;
using AutoBattler.Data.Units;
using AutoBattler.Data.Buffs;

namespace AutoBattler.SaveSystem
{
    [Serializable]
    public class GameData
    {
        public int CurrentRound;
        public int Health;
        public int Gold;
        public int TavernTier;
        public int LevelUpTavernTierCost;

        public BaseUnit[] Storage { get; private set; }
        public BaseUnit[,] Field { get; private set; }
        public List<Buff> Buffs { get; private set; }

        public GameData()
        {
            Health = 0;
            Gold = 0;
            TavernTier = 0;
            LevelUpTavernTierCost = 0;
        }
    }
}
