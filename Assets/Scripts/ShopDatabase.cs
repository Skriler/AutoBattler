using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Database", menuName = "Custom/ShopDatabase")]
public class ShopDatabase : ScriptableObject
{
    [Serializable] 
    public struct ShopUnit
    {
        public BaseUnit prefab;
        public Sprite image;
        public string title;
        public int cost;
    }

    [SerializeField] private List<ShopUnit> shopUnits;

    public List<ShopUnit> GetUnits()
    {
        return shopUnits;
    }

    public int GetUnitsAmount()
    {
        return shopUnits.Count;
    }
}
