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
        public ShopData shop;
        public List<MemberData> bots;

        public GameData()
        {
            player = new MemberData();
            shop = new ShopData();
            bots = new List<MemberData>();
        }
    }
}
