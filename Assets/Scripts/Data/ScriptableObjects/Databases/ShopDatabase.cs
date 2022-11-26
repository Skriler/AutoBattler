using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.Data.ScriptableObjects.Databases
{
    [CreateAssetMenu(
        fileName = "Shop Database",
        menuName = "Custom/Database/ShopDatabase"
        )]
    public class ShopDatabase : ScriptableObject
    {
        [SerializeField] public List<ShopUnitEntity> shopUnits;
    }
}
