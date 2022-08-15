using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIShop : MonoBehaviour
{
    [SerializeField] private UnityEvent<ShopDatabase.ShopUnit> OnUnitBought;

    [SerializeField] private List<UICard> unitCards;
    [SerializeField] private TextMeshProUGUI btnShowShopText;

    private ShopDatabase shopDb;
    private bool isOpen;

    private void Start()
    {
        gameObject.SetActive(isOpen);
        shopDb = GameManager.Instance.shopDatabase;
        GenerateUnitCards();
    }

    public void OnCardClick(UICard card, ShopDatabase.ShopUnit shopUnit)
    {
        //TODO: Check player's gold 
        //TODO: Check is enough space in storage

        card.gameObject.SetActive(false);

        OnUnitBought.Invoke(shopUnit);
    }

    public void OnRefreshClick()
    {
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
