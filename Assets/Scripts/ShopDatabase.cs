using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Units;

[CreateAssetMenu(fileName = "Shop Database", menuName = "Custom/ShopDatabase")]
public class ShopDatabase : ScriptableObject
{
    [Serializable] 
    public struct ShopUnit
    {
        public BaseUnit prefab;
        public Sprite[] sprites;
        public string title;
        public int cost;
        public int tavernTier;
    }

    [SerializeField] private List<ShopUnit> shopUnits;

    public List<ShopUnit> GetUnits()
    {
        return shopUnits;
    }

    public List<ShopUnit> GetUnitsAtTavernTier(int playerTavernTier)
    {
        return shopUnits
            .Where(u => u.tavernTier <= playerTavernTier)
            .ToList();
    }
}
