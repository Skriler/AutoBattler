using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIShop : MonoBehaviour
{
    [SerializeField] private UnityEvent<ShopDatabase.ShopUnit> OnUnitBought;

    [SerializeField] private List<UICard> unitCards;

    private ShopDatabase shopDb;

    private void Start()
    {
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
