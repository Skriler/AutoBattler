﻿using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.SaveSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBattler.Data.Buffs.Containers
{
    public class BotBuffContainer : MemberBuffContainer
    {
        protected void Awake()
        {
            BotsEventManager.OnUnitAddedOnField += ApplyBuffsForUnit;
            BotsEventManager.OnUnitAddedOnField += AddUnitBuffs;
            BotsEventManager.OnUnitRemovedFromField += RemoveBuffsFromUnit;
            BotsEventManager.OnUnitRemovedFromField += RemoveUnitBuffs;
        }

        protected void OnDestroy()
        {
            BotsEventManager.OnUnitAddedOnField -= ApplyBuffsForUnit;
            BotsEventManager.OnUnitAddedOnField -= AddUnitBuffs;
            BotsEventManager.OnUnitRemovedFromField -= RemoveBuffsFromUnit;
            BotsEventManager.OnUnitRemovedFromField -= RemoveUnitBuffs;
        }

        public void AddUnitBuffs(BaseUnit unit, string id)
        {
            if (owner.Id != id)
                return;

            AddUnitBuffs(unit);
        }

        public void RemoveUnitBuffs(BaseUnit unit, string id)
        {
            if (owner.Id != id)
                return;

            RemoveUnitBuffs(unit);
        }

        public void ApplyBuffsForUnit(BaseUnit unit, string id)
        {
            if (owner.Id != id)
                return;

            if (buffs.Count(buff => buff.IsActive()) > 0)
                BuffsEventManager.SendAppliedBuffsForUnit(unit);

            ApplyBuffsForUnit(unit);
        }

        public void RemoveBuffsFromUnit(BaseUnit unit, string id)
        {
            if (owner.Id != id)
                return;

            if (buffs.Count(buff => buff.IsActive()) > 0)
                BuffsEventManager.SendRemovedBuffsFromUnit(unit);

            RemoveBuffsFromUnit(unit);
        }

        public List<Buff> GetRequiredBuffs()
        {
            List<Buff> requiredBuffs = new List<Buff>();
            int minUnitsAmount = 999;

            foreach (Buff buff in buffs)
            {
                if (buff.MaxLevel == buff.CurrentLevel)
                    continue;

                if (buff.UnitsPerLevel - buff.UnitsAmountOnCurrentLevel == minUnitsAmount)
                {
                    requiredBuffs.Add(buff);
                }
                else if (buff.UnitsPerLevel - buff.UnitsAmountOnCurrentLevel < minUnitsAmount)
                {
                    requiredBuffs.Clear();
                    minUnitsAmount = buff.UnitsPerLevel - buff.UnitsAmountOnCurrentLevel;
                    requiredBuffs.Add(buff);
                }
            }

            return requiredBuffs;
        }

        public override void LoadData(GameData data)
        {
            if (data.bots.Count == 0)
                return;

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
