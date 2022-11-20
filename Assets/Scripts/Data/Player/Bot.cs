using UnityEngine;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.UnitsContainers.Containers.Storage;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.Managers;

namespace AutoBattler.Data.Player
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

        public void MakeTurn(int currentRound, int amountOfAliveMembers)
        {
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

        }
    }
}
