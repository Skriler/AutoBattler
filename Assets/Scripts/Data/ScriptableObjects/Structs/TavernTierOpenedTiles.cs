using System;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.ScriptableObjects.Structs
{
    [Serializable]
    public struct TavernTierOpenedTiles
    {
        public int tavernTier;
        public List<Vector2Int> openedTiles;
    }
}
