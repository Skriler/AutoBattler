using System.Collections.Generic;
using AutoBattler.Data.Units;
using UnityEngine;
using AutoBattler.Managers;

namespace AutoBattler.Data.Buffs
{
    public class BuffContainer : Manager<BuffContainer>
    {
        [SerializeField] private List<Buff> buffs;

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

        public void ApplyBuffsForUnit(BaseUnit unit)
        {
            foreach (Buff buff in buffs)
            {
                if (!buff.IsActive())
                    continue;

                float addedPointsAmount = buff.Bonus * buff.CurrentLevel;
                unit.ApplyCharacteristicBonus(buff.TargetCharacteristic, addedPointsAmount);
            }
        }

        public void RemoveBuffsFromUnit(BaseUnit unit)
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
