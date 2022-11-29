using System.Collections.Generic;
using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Members;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.UI.Tooltips;
using AutoBattler.Managers;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.UI.Shop
{
    public class UIShop : MonoBehaviour, IDataPersistence
    {
        [Header("UI Elements")]
        [SerializeField] private UILevelUpButton levelUpButton;
        [SerializeField] private UIRefreshButtonButton refreshButton;

        [Header("Data")]
        [SerializeField] private List<UIUnitCard> unitCards;
        [SerializeField] private Player player;

        [Header("Parameters")]
        [SerializeField] private int refreshCost = 1;

        private ShopUnitsManager shopUnitsManager;

        public bool IsFreezed { get; private set; } = false;

        protected void Awake()
        {
            FightEventManager.OnFightStarted += EndRound;
            FightEventManager.OnFightEnded += StartRound;
            SaveSystemEventManager.OnDataLoaded += Show;
            SaveSystemEventManager.OnNewGameDataCreated += Show;
        }

        protected void OnDestroy()
        {
            FightEventManager.OnFightStarted -= EndRound;
            FightEventManager.OnFightEnded -= StartRound;
            SaveSystemEventManager.OnDataLoaded -= Show;
            SaveSystemEventManager.OnNewGameDataCreated -= Show;
        }

        private void Start()
        {
            shopUnitsManager = ShopUnitsManager.Instance;

            GenerateUnits();

            refreshButton.UpdateDescription(refreshCost);
        }

        public void MouseEnter() => CameraMovement.Instance.IsOnUI = true;

        public void MouseExit() => CameraMovement.Instance.IsOnUI = false;

        public void Show()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            levelUpButton.UpdateDescription(player.LevelUpTavernTierCost);
        }

        public void OnCardClick(UIUnitCard card, ShopUnitEntity shopUnit)
        {
            if (player.Storage.IsFull() || !player.IsEnoughGoldForAction(shopUnit.characteristics.Cost))
            {
                AudioManager.Instance.PlayUnavailableActionSound();
                return;
            }

            player.SpendGold(shopUnit.characteristics.Cost);
            AudioManager.Instance.PlayBuyUnitSound();
            UnitsEventManager.SendUnitBought(shopUnit);

            UIShopUnitTooltip.Instance.Hide();
            UIUnitDescription.Instance.Hide();
            card.gameObject.SetActive(false);
        }

        public void FreezeUnits()
        {
            foreach(UIUnitCard card in unitCards)
                card.Freeze();

            IsFreezed = !IsFreezed;
        }

        public void OnRefreshButtonClick()
        {
            if (!player.IsEnoughGoldForAction(refreshCost))
            {
                AudioManager.Instance.PlayUnavailableActionSound();
                return;
            }

            refreshButton.PlayClickSound();
            player.SpendGold(refreshCost);
            RefreshUnits();
        }

        private void RefreshUnits()
        {
            SetActiveUnitCards();
            GenerateUnits();

            if (IsFreezed)
                FreezeUnits();
        }

        public void LevelUp()
        {
            if (player.IsMaxTavernTier() || !player.IsEnoughGoldForAction(player.LevelUpTavernTierCost))
            {
                AudioManager.Instance.PlayUnavailableActionSound();
                return;
            }

            AudioManager.Instance.PlayLevelUpButtonClickSound();
            player.LevelUpTavernTier();

            if (player.IsMaxTavernTier())
            {
                levelUpButton.gameObject.SetActive(false);
                UIBaseObjectTooltip.Instance.Hide();
            }
            else
            {
                levelUpButton.UpdateDescription(player.LevelUpTavernTierCost);
            }
        }

        private void GenerateUnits()
        {
            List<ShopUnitEntity> shopUnits = shopUnitsManager.GetUnitsAtTavernTier(player.TavernTier);
            unitCards.ForEach(card => GenerateUnit(card, shopUnits));
        }

        private void GenerateUnit(UIUnitCard card, List<ShopUnitEntity> units)
        {
            card.Setup(units[Random.Range(0, units.Count)]);
        }

        private void SetActiveUnitCards()
        {
            foreach (UIUnitCard card in unitCards)
            {
                if (!card.gameObject.activeSelf)
                    card.gameObject.SetActive(true);
            }
        }

        private void AddUnitCardsOnEmptySlots()
        {
            List<ShopUnitEntity> shopUnits = shopUnitsManager.GetUnitsAtTavernTier(player.TavernTier);

            foreach (UIUnitCard card in unitCards)
            {
                if (card.gameObject.activeSelf)
                    continue;

                card.gameObject.SetActive(true);
                GenerateUnit(card, shopUnits);
            }
        }

        private void StartRound()
        {
            if (IsFreezed)
            {
                FreezeUnits();
                AddUnitCardsOnEmptySlots();
            }
            else
            {
                RefreshUnits();
            } 
        }

        private void EndRound()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public void LoadData(GameData data)
        {
            ShopUnitEntity shopUnit;

            foreach (ShopUnitData shopUnitData in data.shop.units)
            {
                if (shopUnitData.isActive)
                {
                    shopUnit = shopUnitsManager.GetShopUnitEntityByTitle(shopUnitData.title);
                    unitCards[shopUnitData.index].Setup(shopUnit);
                }
                else
                {
                    unitCards[shopUnitData.index].gameObject.SetActive(false);
                }
            }

            if (data.shop.isFreezed)
                FreezeUnits();
        }

        public void SaveData(GameData data)
        {
            data.shop.isFreezed = IsFreezed;

            data.shop.units.Clear();
            ShopUnitData shopUnitData;

            for (int i = 0; i < unitCards.Count; ++i)
            {
                shopUnitData = new ShopUnitData(
                    unitCards[i].UnitTitle, 
                    i, 
                    unitCards[i].gameObject.activeSelf
                    );
                data.shop.units.Add(shopUnitData);
            }
        }
    }
}
