using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using AutoBattler.Data.ScriptableObjects;

namespace AutoBattler.UI
{
    public class UIShop : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent<ShopDatabase.ShopUnit> OnUnitBought;

        [Header("UI Elements")]
        [SerializeField] private Button levelUpButton;
        [SerializeField] private TextMeshProUGUI btnShowShopText;

        [Header("Data")]
        [SerializeField] private List<UICard> unitCards;
        [SerializeField] private int rerollCost = 1;
        [SerializeField] private Player player;

        private ShopDatabase shopDb;
        private bool isOpen;

        private void Start()
        {
            shopDb = GameManager.Instance.ShopDb;
            GenerateUnitCards();
            gameObject.SetActive(isOpen);
        }

        public void OnCardClick(UICard card, ShopDatabase.ShopUnit shopUnit)
        {
            if (player.Storage.IsFull())
                return;

            if (!player.IsEnoughGoldForAction(shopUnit.characteristics.Cost))
                return;

            player.SpendGold(shopUnit.characteristics.Cost);
            OnUnitBought.Invoke(shopUnit);

            UICardTooltip.Instance.Hide();
            card.gameObject.SetActive(false);
        }

        public void OnRefreshClick()
        {
            if (!player.IsEnoughGoldForAction(rerollCost))
                return;

            player.SpendGold(rerollCost);
            SetActiveUnitCards();
            GenerateUnitCards();
        }


        public void OnLevelUpClick()
        {
            player.LevelUpTavernTier();

            if (player.IsMaxTavernTier())
                levelUpButton.gameObject.SetActive(false);
        }

        public void OnShowShopClick()
        {
            isOpen = !isOpen;
            gameObject.SetActive(isOpen);

            if (isOpen)
                btnShowShopText.text = "Close Shop";
            else
                btnShowShopText.text = "Open Shop";
        }

        private void GenerateUnitCards()
        {
            List<ShopDatabase.ShopUnit> shopUnits = shopDb.GetUnitsAtTavernTier(player.TavernTier);
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
