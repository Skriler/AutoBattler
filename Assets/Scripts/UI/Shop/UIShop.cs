using System.Collections.Generic;
using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Members;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.UI.Tooltips;
using AutoBattler.Managers;

namespace AutoBattler.UI.Shop
{
    public class UIShop : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private UILevelUpButton levelUpButton;
        [SerializeField] private UIRefreshButtonButton refreshButton;

        [Header("Data")]
        [SerializeField] private List<UICard> unitCards;
        [SerializeField] private Player player;

        [Header("Parameters")]
        [SerializeField] private int refreshCost = 1;

        private ShopDatabase shopDb;

        public bool IsFreezed { get; private set; } = false;

        protected void Awake()
        {
            FightEventManager.OnFightStarted += EndRound;
            FightEventManager.OnFightEnded += StartRound;
            SaveSystemEventManager.OnDataLoaded += GenerateUnits;
        }

        protected void OnDestroy()
        {
            FightEventManager.OnFightStarted -= EndRound;
            FightEventManager.OnFightEnded -= StartRound;
            SaveSystemEventManager.OnDataLoaded -= GenerateUnits;
        }

        private void Start()
        {
            shopDb = GameManager.Instance.ShopDb;
            GenerateUnits();

            refreshButton.UpdateDescription(refreshCost);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            levelUpButton.UpdateDescription(player.LevelUpTavernTierCost);
        }

        public void MouseEnter() => CameraMovement.Instance.IsOnUI = true;

        public void MouseExit() => CameraMovement.Instance.IsOnUI = false;

        public void Show() => gameObject.SetActive(!gameObject.activeSelf);

        public void OnCardClick(UICard card, ShopUnitEntity shopUnit)
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
            foreach(UICard card in unitCards)
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
            List<ShopUnitEntity> shopUnits = shopDb.GetUnitsAtTavernTier(player.TavernTier);
            unitCards.ForEach(card => GenerateUnit(card, shopUnits));
        }

        private void GenerateUnit(UICard card, List<ShopUnitEntity> units)
        {
            card.Setup(units[Random.Range(0, units.Count)]);
        }

        private void SetActiveUnitCards()
        {
            foreach (UICard card in unitCards)
            {
                if (!card.gameObject.activeSelf)
                    card.gameObject.SetActive(true);
            }
        }

        private void AddUnitCardsOnEmptySlots()
        {
            List<ShopUnitEntity> shopUnits = shopDb.GetUnitsAtTavernTier(player.TavernTier);

            foreach (UICard card in unitCards)
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
    }
}
