using System.Collections.Generic;
using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Player;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.UI.Tooltips;
using AutoBattler.Managers;
using Unity.VisualScripting.Antlr3.Runtime;

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

        public void RefreshUnits()
        {
            if (!player.IsEnoughGoldForAction(refreshCost))
            {
                AudioManager.Instance.PlayUnavailableActionSound();
                return;
            }

            if (gameObject.activeSelf)
                refreshButton.PlayClickSound();

            player.SpendGold(refreshCost);

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
                RefreshUnits();
        }

        private void EndRound()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }
}
