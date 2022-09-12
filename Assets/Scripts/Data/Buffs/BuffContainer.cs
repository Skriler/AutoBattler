using System.Collections.Generic;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.Data.Buffs
{
    public class BuffContainer
    {
        private List<BaseBuff> allBuffs;

        private BuffDatabase buffDb;

        public BuffContainer()
        {
            UnitsEventManager.OnUnitAddedOnField += AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField += RemoveUnitBuffs;

            buffDb = GameManager.Instance.BuffDb;
            allBuffs = buffDb.GetAllBuffs();
            ResetBuffs();
        }

        ~BuffContainer()
        {
            UnitsEventManager.OnUnitAddedOnField -= AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField -= RemoveUnitBuffs;
        }

        public void ResetBuffs()
        {
            foreach (BaseBuff buff in allBuffs)
            {
                buff.ResetBuff();
            }
        }

        public void AddUnitBuffs(BaseUnit unit)
        {
            foreach (BaseBuff buff in allBuffs)
            {
                buff.AddBuff(unit);
            }

            //player.DebugBuffs();
        }

        public void RemoveUnitBuffs(BaseUnit unit)
        {
            foreach (BaseBuff buff in allBuffs)
            {
                buff.RemoveBuff(unit);
            }

            //player.DebugBuffs();
        }
    }
}
