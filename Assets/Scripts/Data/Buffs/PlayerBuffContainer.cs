using AutoBattler.EventManagers;

namespace AutoBattler.Data.Buffs
{
    public class PlayerBuffContainer : BuffContainer
    {
        protected override void Awake()
        {
            base.Awake();

            UnitsEventManager.OnUnitAddedOnField += ApplyBuffsForUnit;
            UnitsEventManager.OnUnitAddedOnField += AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField += RemoveBuffsFromUnit;
            UnitsEventManager.OnUnitRemovedFromField += RemoveUnitBuffs;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitAddedOnField -= ApplyBuffsForUnit;
            UnitsEventManager.OnUnitAddedOnField -= AddUnitBuffs;
            UnitsEventManager.OnUnitRemovedFromField -= RemoveBuffsFromUnit;
            UnitsEventManager.OnUnitRemovedFromField -= RemoveUnitBuffs;
        }
    }
}
