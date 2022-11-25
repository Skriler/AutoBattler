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

        private ShopDatabase shopDb;

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

        public void MakeTurn(int currentRound, int amountOfAliveMembers)
        {
            if (shopDb == null)
                shopDb = GameManager.Instance.ShopDb;

            switch (currentRound)
            {
                case < (int)GamePhase.EarlyGame:
                    EarlyGameRoundLogic(currentRound);
                    break;
                case < (int)GamePhase.PreMidGame:
                    PreMidGameRoundLogic(currentRound);
                    break;
                case < (int)GamePhase.MidGame:
                    MidGameRoundLogic(currentRound);
                    break;
                case < (int)GamePhase.PreLateGame:
                    PreLateGameRoundLogic();
                    break;
                case < (int)GamePhase.LateGame:
                    LateGameRoundLogic();
                    break;
            }
        }

        private void EarlyGameRoundLogic(int currentRound)
        {
            while (Gold != 0)
            {
                BuyUnit();
            }
            
            ModifyFieldUnits();
        }

        private void PreMidGameRoundLogic(int currentRound)
        {
            if (currentRound == (int)GamePhase.EarlyGame)
                LevelUpTavernTier();


        }

        private void MidGameRoundLogic(int currentRound)
        {
            if (currentRound == (int)GamePhase.PreMidGame)
                LevelUpTavernTier();


        }

        private void PreLateGameRoundLogic()
        {

        }

        private void LateGameRoundLogic()
        {

        }
            

        private void BuyUnit()
        {
            ShopUnitEntity shopUnit;

            if (Storage.IsEmpty() && Field.IsEmpty())
                shopUnit = shopDb.GetRandomShopUnitEntityAtTavernTier(TavernTier, Gold);
            else
                shopUnit = GetRequiredShopUnitEntity();

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
                requiredUnit = shopDb.GetUnitWithRace(
                    (requiredBuff as RaceBuff).Race,
                    TavernTier,
                    Gold
                    );
            }
            else
            {
                requiredUnit = shopDb.GetUnitWithSpecification(
                    (requiredBuff as SpecificationBuff).Specification,
                    TavernTier,
                    Gold
                    );
            }

            if (requiredUnit.Equals(default(ShopUnitEntity)))
                requiredUnit = shopDb.GetRandomShopUnitEntityAtTavernTier(TavernTier, Gold);

            return requiredUnit;
        }

        private void ModifyFieldUnits()
        {
            int unitsAmountInStorage = Storage.GetUnitsAmount();
            int fieldOpenedCellsAmount = Field.GetOpenedCellsAmount();

            if (unitsAmountInStorage <= fieldOpenedCellsAmount)
                PlaceAllUnitsOnField(unitsAmountInStorage);
        }

        private void PlaceAllUnitsOnField(int unitsAmount)
        {
            BaseUnit unit;
            for (int i = 0; i < unitsAmount; ++i)
            {
                unit = Storage.GetUnit();
                Storage.RemoveUnit(unit);
                Field.AddUnit(unit);
            }
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
