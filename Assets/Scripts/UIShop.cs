using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public List<UICard> unitCards;

    private ShopDatabase shopDb;

    public void Start()
    {
        shopDb = GameManager.Instance.shopDatabase;
        GenerateUnitCards();
    }

    public void GenerateUnitCards()
    {
        for(int i = 0; i < unitCards.Count; ++i)
        {
            unitCards[i].Setup(shopDb.shopUnits[Random.Range(0, shopDb.shopUnits.Count)], this);
        }
    }

    public void OnCardClick(ShopDatabase.ShopUnit shopUnit)
    {
        Debug.Log("On " + shopUnit.title + " card clicked");
    }

    public void OnRefreshClick()
    {
        GenerateUnitCards();
    }
}
