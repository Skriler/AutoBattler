using AutoBattler.Data.Units;
using AutoBattler.SaveSystem.Data;
using System.Linq;

namespace AutoBattler.UnitsContainers.Containers.Storage
{ 
    public class BotStorageContainer : MemberStorageContainer
    {
        public bool IsEmpty() => GetUnitsAmount() == 0;

        public int GetUnitsAmount()
        {
            int unitsAmount = 0;

            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    continue;

                ++unitsAmount;
            }

            return unitsAmount;
        }

        public BaseUnit GetUnit()
        {
            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    continue;

                return units[i];
            }

            return null;
        }

        public override void LoadData(GameData data)
        {
            MemberData memberData = data.bots.Where(b => b.id == owner.Id).First();

            LoadDataFromMemberData(memberData);
        }

        public override void SaveData(GameData data)
        {
            MemberData memberData;
            if (data.bots.Exists(b => b.id == owner.Id))
            {
                memberData = data.bots.Where(b => b.id == owner.Id).First();
            }
            else
            {
                memberData = new MemberData();
                memberData.id = owner.Id;
                data.bots.Add(memberData);
            }

            SaveDataToMemberData(memberData);
        }
    }
}
