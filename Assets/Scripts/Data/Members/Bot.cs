using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.UnitsContainers.Containers.Storage;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.Buffs;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.Data.Units;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Data.Members
{
    public class Bot : Member
    {
        public BotStorageContainer Storage { get; protected set; }
        public BotFieldContainer Field { get; protected set; }

        private ShopUnitsManager shopUnitsManager;

        protected override void Awake()
        {
            base.Awake();

            Storage = transform.GetComponentInChildren<BotStorageContainer>();
            Field = transform.GetComponentInChildren<BotFieldContainer>();
        }

        public override MemberFieldContainer GetFieldContainer() => Field;

        public override void LevelUpTavernTier()
        {
            if (IsMaxTavernTier() || !IsEnoughGoldForAction(LevelUpTavernTierCost))
                return;

            base.LevelUpTavernTier();
            BotsEventManager.SendTavernTierIncreased(TavernTier, Id);
            LevelUpTavernTierCost = Field.GetOpenedCellsAmount();
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            BotsEventManager.SendHealthAmountChanged(Health, Id);
        }

        public void MakeTurn()
        {
            if (shopUnitsManager == null)
                shopUnitsManager = ShopUnitsManager.Instance;

            if (Field.IsFull() && IsEnoughGoldForAction(LevelUpTavernTierCost) && Random.value < 0.5f)
                LevelUpTavernTier();

            ShopUnitEntity shopUnit;
            while (Gold != 0)
            {
                shopUnit = GetRequiredUnit();

                if (!IsEnoughGoldForAction(shopUnit.characteristics.Cost) || Storage.IsFull())
                    break;

                BuyUnit(shopUnit);

                if (Storage.GetUnitsAmount() >= 4 && IsEnoughGoldForAction(LevelUpTavernTierCost))
                    LevelUpTavernTier();
            }

            ModifyFieldUnits();
        }   

        private ShopUnitEntity GetRequiredUnit()
        {
            ShopUnitEntity shopUnit = 
                Storage.IsEmpty() && Field.IsEmpty() ?
                shopUnitsManager.GetRandomShopUnitEntityAtTavernTier(TavernTier, Gold) : 
                GetRequiredShopUnitEntity();

            return shopUnit;
        }

        private void BuyUnit(ShopUnitEntity shopUnit)
        {
            if (!IsEnoughGoldForAction(shopUnit.characteristics.Cost) || Storage.IsFull())
                return;

            SpendGold(shopUnit.characteristics.Cost);
            Storage.AddUnit(shopUnit);
        }

        private ShopUnitEntity GetRequiredShopUnitEntity()
        {
            ShopUnitEntity requiredUnit;

            List<Buff> requiredBuffs = Field.Buffs.GetRequiredBuffs();
            Buff requiredBuff = requiredBuffs[Random.Range(0, requiredBuffs.Count)];

            if (requiredBuff is RaceBuff)
            {
                requiredUnit = shopUnitsManager.GetUnitWithRace(
                    (requiredBuff as RaceBuff).Race,
                    TavernTier,
                    Gold
                    );
            }
            else
            {
                requiredUnit = shopUnitsManager.GetUnitWithSpecification(
                    (requiredBuff as SpecificationBuff).Specification,
                    TavernTier,
                    Gold
                    );
            }

            if (requiredUnit.Equals(default(ShopUnitEntity)))
                requiredUnit = shopUnitsManager.GetRandomShopUnitEntityAtTavernTier(TavernTier, Gold);

            return requiredUnit;
        }

        private void ModifyFieldUnits()
        {
            int unitsAmountInStorage = Storage.GetUnitsAmount();
            int freeCellsAmountOnField = Field.GetFreeCellsAmount();

            BaseUnit unit;
            for (int i = 0; i < unitsAmountInStorage; ++i)
            {
                if (freeCellsAmountOnField <= 0)
                {
                    unit = Field.GetWeakestUnit();
                    Field.RemoveUnit(unit);
                    Storage.AddUnit(unit);

                    ++freeCellsAmountOnField;
                }

                if (freeCellsAmountOnField > 0)
                {
                    unit = Storage.GetStrongestUnit();
                    Storage.RemoveUnit(unit);
                    Field.AddUnit(unit);

                    --freeCellsAmountOnField;
                }
            }

            unitsAmountInStorage = Storage.GetUnitsAmount();
            while (unitsAmountInStorage >= 2)
            {
                SellUnitFromStorage(Storage.GetWeakestUnit());
                --unitsAmountInStorage;
            }
        }

        private void SellUnitFromStorage(BaseUnit unit)
        {
            Storage.RemoveUnit(unit);
            Destroy(unit.gameObject);
            GainGold(1);
        }

        public override void LoadData(GameData data)
        {
            if (data.bots.Count == 0)
                return;

            MemberData memberData = data.bots.Where(b => b.id == Id).First();

            LoadDataFromMemberData(memberData);

            BotsEventManager.SendHealthAmountChanged(Health, id);
            BotsEventManager.SendTavernTierIncreased(TavernTier, Id);
        }

        public override void SaveData(GameData data)
        {
            MemberData memberData;
            if (data.bots.Exists(b => b.id == Id))
            {
                memberData = data.bots.Where(b => b.id == Id).First();
            }
            else
            {
                memberData = new MemberData();
                data.bots.Add(memberData);
            }

            SaveDataToMemberData(memberData);
        }
    }
}
