using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.Data.Buffs
{
    public class BuffContainer : MonoBehaviour
    {
        [SerializeField] protected List<Buff> buffs;

        public List<Buff> GetBuffs() => buffs;

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

        public virtual void ApplyBuffsForUnit(BaseUnit unit)
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.IsActive())
                    continue;

                float addedPointsAmount = buff.Bonus * buff.CurrentLevel;
                unit.ApplyCharacteristicBonus(buff.TargetCharacteristic, addedPointsAmount);    
            }
        }

        public virtual void RemoveBuffsFromUnit(BaseUnit unit)
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.IsActive())
                    continue;

                float removedPointsAmount = -buff.Bonus * buff.CurrentLevel;
                unit.ApplyCharacteristicBonus(buff.TargetCharacteristic, removedPointsAmount);
            }
        }
    }
}
