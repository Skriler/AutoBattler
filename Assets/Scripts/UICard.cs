using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICard : MonoBehaviour
{
    //public GameObject prefab;
    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI cost;

    private ShopDatabase.ShopUnit shopUnit;
    private UIShop shop;
    
    public void Setup(ShopDatabase.ShopUnit shopUnit, UIShop shop)
    {
        //shopUnit.prefab.transform.position = prefab.transform.position;
        //prefab = shopUnit.prefab;

        //GameObject canvas = GameObject.Find("UI");
        //prefab.transform.SetParent(canvas.transform, false);

        image.sprite = shopUnit.image;
        title.text = shopUnit.title;
        cost.text = shopUnit.cost.ToString();

        this.shopUnit = shopUnit;
        this.shop = shop;

        //Instantiate(prefab, prefab.transform.position, Quaternion.identity);
        //Instantiate(prefab, new Vector2(0, 0), Quaternion.identity);
    }

    public void OnClick()
    {
        shop.OnCardClick(shopUnit);
    }
}
