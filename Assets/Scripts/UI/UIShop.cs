using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public UnityEvent<ShopDatabase.ShopUnit> OnUnitBought;

    public List<UICard> unitCards;

    private ShopDatabase shopDb;

    public void Start()
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
        for (int i = 0; i < unitCards.Count; ++i)
        {
            unitCards[i].Setup(shopDb.shopUnits[Random.Range(0, shopDb.shopUnits.Count)]);
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
