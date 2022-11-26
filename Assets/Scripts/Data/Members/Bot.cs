using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.UnitsContainers.Containers.Storage;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.Enums;
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
        private UnitRace targetRace;
        private UnitSpecification targetSpecification;

        protected override void Awake()
        {
            base.Awake();

            Storage = transform.GetComponentInChildren<BotStorageContainer>();
            Field = transform.GetComponentInChildren<BotFieldContainer>();
            targetRace = GetRandomRace();
            targetSpecification = GetRandomSpecification();
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

        public void MakeTurn(int currentRound, int amountOfAliveMembers)
        {
            if (shopUnitsManager == null)
                shopUnitsManager = ShopUnitsManager.Instance;

            //switch (currentRound)
            //{
            //    case < (int)GamePhase.EarlyGame:
            //        EarlyGameRoundLogic();
            //        break;
            //    case < (int)GamePhase.PreMidGame:
            //        PreMidGameRoundLogic();
            //        break;
            //    case < (int)GamePhase.MidGame:
            //        MidGameRoundLogic();
            //        break;
            //    case < (int)GamePhase.PreLateGame:
            //        PreLateGameRoundLogic();
            //        break;
            //    case < (int)GamePhase.LateGame:
            //        LateGameRoundLogic();
            //        break;
            //}

            if (Field.IsFull() && IsEnoughGoldForAction(LevelUpTavernTierCost))
                LevelUpTavernTier();

            ShopUnitEntity shopUnit;
            while (Gold != 0)
            {
                shopUnit = GetRequiredUnit();

                if (!IsEnoughGoldForAction(shopUnit.characteristics.Cost))
                    break;

                BuyUnit(shopUnit);
            }

            ModifyFieldUnits();
        }

        //private void EarlyGameRoundLogic()
        //{
        //    while (Gold != 0)
        //    {
        //        BuyUnit();
        //    }
            
        //    ModifyFieldUnits();
        //}

        //private void PreMidGameRoundLogic()
        //{
        //    if (Field.IsFull() && IsEnoughGoldForAction(LevelUpTavernTierCost))
        //        LevelUpTavernTier();

        //    while (Gold != 0)
        //    {
        //        BuyUnit();
        //    }

        //    ModifyFieldUnits();
        //}

        //private void MidGameRoundLogic()
        //{
        //    if (Field.IsFull() && IsEnoughGoldForAction(LevelUpTavernTierCost))
        //        LevelUpTavernTier();

        //    while (Gold != 0)
        //    {
        //        BuyUnit();
        //    }

        //    ModifyFieldUnits();
        //}

        //private void PreLateGameRoundLogic()
        //{
        //    if (Field.IsFull() && IsEnoughGoldForAction(LevelUpTavernTierCost))
        //        LevelUpTavernTier();



        //    while (Gold != 0)
        //    {
        //        BuyUnit();
        //    }

        //    ModifyFieldUnits();
        //}

        //private void LateGameRoundLogic()
        //{
        //    if (Field.IsFull() && IsEnoughGoldForAction(LevelUpTavernTierCost))
        //        LevelUpTavernTier();

        //    ShopUnitEntity shopUnit;
        //    while (Gold != 0)
        //    {
        //        shopUnit = GetRequiredUnit();

        //        if (!IsEnoughGoldForAction(shopUnit.characteristics.Cost))
        //            break;

        //        BuyUnit(shopUnit);
        //    }

        //    ModifyFieldUnits();
        //}
            

        private ShopUnitEntity GetRequiredUnit()
        {
            ShopUnitEntity shopUnit = 
                Storage.IsEmpty() && Field.IsEmpty() ?
                shopUnitsManager.GetRandomShopUnitEntityAtTavernTierAndLower(TavernTier, Gold) : 
                GetRequiredShopUnitEntity();

            return shopUnit;
        }

        private void BuyUnit(ShopUnitEntity shopUnit)
        {
            if (!IsEnoughGoldForAction(shopUnit.characteristics.Cost))
                return;

            SpendGold(shopUnit.characteristics.Cost);
            Storage.AddUnit(shopUnit);
        }

        private ShopUnitEntity GetRequiredShopUnitEntity()
        {
            ShopUnitEntity requiredUnit;

            List<Buff> requiredBuffs = Field.Buffs.GetRequiredBuffs();
            Buff requiredBuff = requiredBuffs[UnityEngine.Random.Range(0, requiredBuffs.Count)];

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
                requiredUnit = shopUnitsManager.GetRandomShopUnitEntityAtTavernTierAndLower(TavernTier, Gold);

            return requiredUnit;
        }

        private void ModifyFieldUnits()
        {
            int unitsAmountInStorage = Storage.GetUnitsAmount();
            int freeCellsAmountOnField = Field.GetFreeCellsAmount();

            BaseUnit unit;
            for (int i = 0; i < unitsAmountInStorage; ++i)
            {
                if (freeCellsAmountOnField == 0)
                {
                    unit = Field.GetWeakestUnit();
                    Field.RemoveUnit(unit);
                    Storage.AddUnit(unit);
                }

                unit = Storage.GetStrongestUnit();
                Storage.RemoveUnit(unit);
                Field.AddUnit(unit);

                --unitsAmountInStorage;
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

        private UnitRace GetRandomRace()
        {
            Array races = Enum.GetValues(typeof(UnitRace));
            return (UnitRace)races
                .GetValue(UnityEngine.Random.Range(0, races.Length));
        }

        private UnitSpecification GetRandomSpecification()
        {
            Array specifications = Enum.GetValues(typeof(UnitSpecification));
            return (UnitSpecification)specifications
                .GetValue(UnityEngine.Random.Range(0, specifications.Length));
        }

        public override void LoadData(GameData data)
        {
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
