using UnityEngine;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.UnitsContainers.Containers.Storage;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.Data.Units;

namespace AutoBattler.Data.Members
{
    public class Bot : Member
    {
        public BotStorageContainer Storage { get; protected set; }
        public BotFieldContainer Field { get; protected set; }

        private ShopDatabase shopDb;

        protected override void Start()
        {
            base.Start();

            Storage = transform.GetComponentInChildren<BotStorageContainer>();
            Field = transform.GetComponentInChildren<BotFieldContainer>();
            shopDb = GameManager.Instance.ShopDb;
        }

        protected void LevelUpTavernTier()
        {
            if (IsMaxTavernTier() || !IsEnoughGoldForAction(LevelUpTavernTierCost))
                return;

            SpendGold(LevelUpTavernTierCost);

            ++TavernTier;
            BotsEventManager.OnTavernTierIncreased(TavernTier, Id);
            LevelUpTavernTierCost = Field.GetOpenedCellsAmount();
        }

        protected void SpendGold(int actionCost)
        {
            if (Gold - actionCost < 0)
                return;

            Gold -= actionCost;
        }

        public void MakeTurn(int currentRound, int amountOfAliveMembers)
        {
            //Gold = 99;
            //LevelUpTavernTier();
            //BuyRandomUnit();
            //BuyRandomUnit();

            BuyRandomUnit();
            ModifyFieldUnits();
        }

        private void BuyRandomUnit()
        {
            ShopUnitEntity shopUnit = shopDb.GetRandomShopUnitEntityAtTavernTier(TavernTier);
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
    }
}
