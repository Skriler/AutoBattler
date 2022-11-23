using System.Linq;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Data.Buffs.Containers
{
    public class PlayerBuffContainer : MemberBuffContainer
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

        public override void LoadData(GameData data)
        {
            LoadDataFromMemberData(data.player);
        }

        public override void SaveData(GameData data)
        {
            SaveDataToMemberData(data.player);
        }
    }
}
