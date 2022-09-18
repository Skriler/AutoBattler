using System.Collections.Generic;
using AutoBattler.Data.Units;
using UnityEngine;
using AutoBattler.EventManagers;

namespace AutoBattler.Data.Buffs
{
    public class BuffContainer : MonoBehaviour
    {
        [SerializeField] private List<Buff> buffs;

        private void OnEnable()
        {
            UnitsEventManager.OnUnitAddedOnField += AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField += RemoveUnitBuffs;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitAddedOnField -= AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField -= RemoveUnitBuffs;
        }

        public void ResetBuffs()
        {
            foreach (Buff buff in buffs)
                buff.ResetProgress();
        }

        public void AddUnitBuffs(BaseUnit unit)
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.IsUnitPassCheck(unit))
                    continue;

                buff.AddBuff(unit);
            }
        }

        public void RemoveUnitBuffs(BaseUnit unit)
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.IsUnitPassCheck(unit))
                    continue;

                buff.RemoveBuff(unit);
            }
        }
    }
}
