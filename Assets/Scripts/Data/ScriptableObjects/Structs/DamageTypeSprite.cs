using System;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.ScriptableObjects.Structs
{
    [Serializable]
    public struct DamageTypeSprite
    {
        public DamageType damageType;
        public Sprite sprite;
    }
}
