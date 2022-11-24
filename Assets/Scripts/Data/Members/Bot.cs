using UnityEngine;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.UnitsContainers.Containers.Storage;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.Data.Units;
using AutoBattler.SaveSystem.Data;
using System.Linq;

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

            if (!IsAlive())
                Death();
        }

        public void MakeTurn(int currentRound, int amountOfAliveMembers)
        {
            if (shopDb == null)
                shopDb = GameManager.Instance.ShopDb;

            //Gold = 99;
            //LevelUpTavernTier();
            //BuyRandomUnit();
            //BuyRandomUnit();

            BuyRandomUnit();
            ModifyFieldUnits();
        }

        private void BuyRandomUnit()
        {
            ShopUnitEntity shopUnit = shopDb.GetRandomShopUnitEntityAtTavernTier(TavernTier, 99);
            Storage.AddUnit(shopUnit);
        }

        private void ModifyFieldUnits()
        {
            int unitsAmountInStorage = Storage.GetUnitsAmount();
            int fieldOpenedCellsAmount = Field.GetOpenedCellsAmount();

            if (unitsAmountInStorage <= fieldOpenedCellsAmount)
            {
                BaseUnit unit;
                for (int i = 0; i < unitsAmountInStorage; ++i)
                {
                    unit = Storage.GetUnit();
                    Storage.RemoveUnit(unit);
                    Field.AddUnit(unit);
                }
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
