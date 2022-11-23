using System;
using System.Collections.Generic;
using AutoBattler.Data.Enums;

namespace AutoBattler.SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public GameMode gameMode;
        public int currentRound;
        public MemberData player;
        public List<MemberData> bots;

        public GameData()
        {
            player = new MemberData();
            bots = new List<MemberData>();
        }
    }
}
