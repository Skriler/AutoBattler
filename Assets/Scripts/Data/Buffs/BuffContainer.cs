using System.Collections.Generic;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.EventManagers;

namespace AutoBattler.Data.Buffs
{
    public class BuffContainer
    {
        public List<RaceBuff> RaceBuffs;
        public List<SpecificationBuff> SpecificationBuffs;

        public BuffContainer()
        {
            UnitsEventManager.OnUnitBought += AddBuffs;

            BuffDatabase buffDb = GameManager.Instance.BuffDb;

            SpecificationBuffs = buffDb.GetSpecificationBuffs();
            RaceBuffs = buffDb.GetRaceBuffs();
        }

        ~BuffContainer()
        {
            UnitsEventManager.OnUnitBought -= AddBuffs;
        }

        public void AddBuffs(ShopUnitEntity unit)
        {
            foreach(SpecificationBuff buff in SpecificationBuffs)
            {
                if (buff.Specification != unit.characteristics.Specification)
                    continue;

                buff.AddUnitsAmount();
            }

            foreach (RaceBuff buff in RaceBuffs)
            {
                if (buff.Race != unit.characteristics.Race)
                    continue;

                buff.AddUnitsAmount();
            }
        }
    }
}
