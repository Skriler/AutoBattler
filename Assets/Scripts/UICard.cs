using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICard : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI cost;
    public Image unitImage;
    public GameObject prefab;
    public GameObject unitCard;

    private ShopDatabase.ShopUnit shopUnit;
    private UIShop shop;
    
    public void Setup(ShopDatabase.ShopUnit shopUnit, UIShop shop)
    {
        title.text = shopUnit.title;
        cost.text = shopUnit.cost.ToString();
        unitImage.sprite = shopUnit.image;

        this.shopUnit = shopUnit;
        this.shop = shop;

        shopUnit.prefab.transform.position = prefab.transform.position;
        prefab = shopUnit.prefab;

        GameObject unit = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
        unit.transform.SetParent(unitCard.transform);
        unit.transform.localScale = new Vector2(120, 120);
        unit.transform.SetAsLastSibling();
    }

    public void OnClick()
    {
        shop.OnCardClick(shopUnit);
    }
}
