using System.Linq;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Data.Buffs
{
    public class PlayerBuffContainer : BuffContainer, IDataPersistence
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

        public void LoadData(GameData data)
        {
            Buff buff;

            foreach (BuffData buffData in data.player.buffs)
            {
                buff = GetBuffByTitle(buffData.title);
                buff.SetBuffDataСharacteristics(buffData);
            }
        }

        public void SaveData(GameData data)
        {
            data.player.buffs.Clear();

            foreach (Buff buff in buffs)
                data.player.buffs.Add(new BuffData(buff));
        }
    }
}
