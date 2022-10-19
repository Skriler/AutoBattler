using System.Collections.Generic;
using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Player;
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

        public bool IsOpen { get; private set; } = false;
        public bool IsFreezed { get; private set; } = false;

        protected void Awake()
        {
            FightEventManager.OnFightStarted += EndRound;
            FightEventManager.OnFightEnded += StartRound;
        }

        protected void OnDestroy()
        {
            FightEventManager.OnFightStarted -= EndRound;
            FightEventManager.OnFightEnded -= StartRound;
        }

        private void Start()
        {
            shopDb = GameManager.Instance.ShopDb;
            GenerateUnitCards();

            levelUpButton.UpdateDescription(player.LevelUpTavernTierCost);
            refreshButton.UpdateDescription(refreshCost);

            gameObject.SetActive(IsOpen);
        }

        public void MouseEnter() => CameraMovement.Instance.IsOnUI = true;

        public void MouseExit() => CameraMovement.Instance.IsOnUI = false;

        public void OnCardClick(UICard card, ShopUnitEntity shopUnit)
        {
            if (player.Storage.IsFull() || !player.IsEnoughGoldForAction(shopUnit.characteristics.Cost))
            {
                AudioManager.Instance.PlayUnavailableActionSound();
                return;
            }

            player.SpendGold(shopUnit.characteristics.Cost);
            AudioManager.Instance.PlayBuyUnitSound();
            UnitsEventManager.OnUnitBought(shopUnit);

            UIShopUnitTooltip.Instance.Hide();
            card.gameObject.SetActive(false);
        }

        public void FreezeUnits()
        {
            foreach(UICard card in unitCards)
                card.Freeze();

            IsFreezed = !IsFreezed;
        }

        public void RefreshShop()
        {
            if (!player.IsEnoughGoldForAction(refreshCost))
            {
                AudioManager.Instance.PlayUnavailableActionSound();
                return;
            }

            refreshButton.PlayClickSound();
            player.SpendGold(refreshCost);

            SetActiveUnitCards();
            GenerateUnitCards();

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

        public void ShowShop()
        {
            IsOpen = !IsOpen;
            gameObject.SetActive(IsOpen);
        }

        private void GenerateUnitCards()
        {
            List<ShopUnitEntity> shopUnits = shopDb.GetUnitsAtTavernTier(player.TavernTier);
            int unitsAmount = shopUnits.Count;

            for (int i = 0; i < unitCards.Count; ++i)
            {
                unitCards[i].Setup(shopUnits[Random.Range(0, unitsAmount)]);
            }
        }

        private void SetActiveUnitCards()
        {
            foreach (UICard card in unitCards)
            {
                if (!card.gameObject.activeSelf)
                    card.gameObject.SetActive(true);
            }
        }

        private void StartRound()
        {
            if (IsFreezed)
                FreezeUnits();
            else
                RefreshShop();
        }

        private void EndRound()
        {
            if (IsOpen)
            {
                gameObject.SetActive(false);
                IsOpen = false;
            }
        }
    }
}
