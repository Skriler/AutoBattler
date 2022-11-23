using UnityEngine;
using AutoBattler.Data.Members;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Managers;

namespace AutoBattler.Data.Buffs.Containers
{
    public abstract class MemberBuffContainer : BuffContainer, IDataPersistence
    {
        [SerializeField] protected Member owner;

        public abstract void LoadData(GameData data);

        public abstract void SaveData(GameData data);

        public void LoadDataFromMemberData(MemberData memberData)
        {
            Buff buff;
            foreach (BuffData buffData in memberData.buffs)
            {
                buff = GetBuffByTitle(buffData.title);
                buff.SetBuffDataСharacteristics(buffData);
            }
        }

        public void SaveDataToMemberData(MemberData memberData)
        {
            memberData.buffs.Clear();

            foreach (Buff buff in buffs)
                memberData.buffs.Add(new BuffData(buff));
        }
    }
}
