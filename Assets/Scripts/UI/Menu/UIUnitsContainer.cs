using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AutoBattler.Managers;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.UI.Menu
{
    public class UIUnitsContainer : MonoBehaviour
    {
        [SerializeField] private List<UIUnit> units;
        [SerializeField] private ShopDatabase shopDb;

        private void Start()
        {
            GenerateUIUnits();
        }

        private void GenerateUIUnits()
        {
            List<ShopUnitEntity> shopUnits = shopDb.GetUnits();
            int unitsAmount = shopUnits.Count;

            for (int i = 0; i < units.Count; ++i)
                units[i].Setup(shopUnits[Random.Range(0, unitsAmount)]);
        }
    }
}
