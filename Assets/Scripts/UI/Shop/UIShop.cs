using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AutoBattler.EventManagers;
using AutoBattler.Data.Players;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.UI.Tooltips;
using AutoBattler.Managers;

namespace AutoBattler.UI.Shop
{
    public class UIShop : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button levelUpButton;

        [Header("Data")]
        [SerializeField] private List<UICard> unitCards;
        [SerializeField] private Player player;

        [Header("Parameters")]
        [SerializeField] private int rerollCost = 1;

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
            gameObject.SetActive(IsOpen);
        }

        public void MouseEnter() => CameraMovement.Instance.IsOnUI = true;

        public void MouseExit() => CameraMovement.Instance.IsOnUI = false;

        public void OnCardClick(UICard card, ShopUnitEntity shopUnit)
        {
            if (player.Storage.IsFull())
                return;

            if (!player.IsEnoughGoldForAction(shopUnit.characteristics.Cost))
                return;

            player.SpendGold(shopUnit.characteristics.Cost);
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
            if (!player.IsEnoughGoldForAction(rerollCost))
                return;

            player.SpendGold(rerollCost);
            SetActiveUnitCards();
            GenerateUnitCards();
        }

        public void LevelUp()
        {
            player.LevelUpTavernTier();

            if (player.IsMaxTavernTier())
                levelUpButton.gameObject.SetActive(false);
        }

        public void ShowShop()
        {
            IsOpen = !IsOpen;
            gameObject.SetActive(IsOpen);
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
    }
}
