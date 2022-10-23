using System.Linq;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;

namespace AutoBattler.Data.Buffs
{
    public class PlayerBuffContainer : BuffContainer
    {
        protected void Awake()
        {
            UnitsEventManager.OnUnitAddedOnField += ApplyBuffsForUnit;
            UnitsEventManager.OnUnitAddedOnField += AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField += RemoveBuffsFromUnit;
            UnitsEventManager.OnUnitRemovedFromField += RemoveUnitBuffs;
        }

        protected void OnDestroy()
        {
            UnitsEventManager.OnUnitAddedOnField -= ApplyBuffsForUnit;
            UnitsEventManager.OnUnitAddedOnField -= AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField -= RemoveBuffsFromUnit;
            UnitsEventManager.OnUnitRemovedFromField -= RemoveUnitBuffs;
        }

        public override void ApplyBuffsForUnit(BaseUnit unit)
        {
            if (buffs.Count(buff => buff.IsActive()) > 0)
                BuffsEventManager.SendAppliedBuffsForUnit(unit);

            base.ApplyBuffsForUnit(unit);
        }

        public override void RemoveBuffsFromUnit(BaseUnit unit)
        {
            if (buffs.Count(buff => buff.IsActive()) > 0)
                BuffsEventManager.SendRemovedBuffsFromUnit(unit);

            base.RemoveBuffsFromUnit(unit);
        }
    }
}
