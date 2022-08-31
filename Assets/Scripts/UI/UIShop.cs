using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace AutoBattler.UI
{
    public class UIShop : MonoBehaviour
    {
        [SerializeField] private UnityEvent<ShopDatabase.ShopUnit> OnUnitBought;

        [SerializeField] private int rerollCost = 1;
        [SerializeField] private List<UICard> unitCards;
        [SerializeField] private Player player;
        [SerializeField] private TextMeshProUGUI btnShowShopText;

        private ShopDatabase shopDb;
        private bool isOpen;

        private void Start()
        {
            gameObject.SetActive(isOpen);
            shopDb = GameManager.Instance.GetShopDb();
            GenerateUnitCards();
        }

        public void OnCardClick(UICard card, ShopDatabase.ShopUnit shopUnit)
        {
            if (player.Storage.IsFull())
                return;

            if (!player.IsEnoughGoldForAction(shopUnit.cost))
                return;

            player.SpendGold(shopUnit.cost);
            card.gameObject.SetActive(false);
            OnUnitBought.Invoke(shopUnit);
        }

        public void OnRefreshClick()
        {
            if (!player.IsEnoughGoldForAction(rerollCost))
                return;

            player.SpendGold(rerollCost);
            SetActiveUnitCards();
            GenerateUnitCards();
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
            List<ShopDatabase.ShopUnit> shopUnits = shopDb.GetUnits();
            int unitsAmount = shopDb.GetUnitsAmount();

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
