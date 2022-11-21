using System;
using System.Collections.Generic;

namespace AutoBattler.SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public int currentRound;
        public PlayerData player;
        public List<MemberData> bots;

        public GameData()
        {
            player = new PlayerData();
            bots = new List<MemberData>();
        }
    }
}
