using System;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.ScriptableObjects.Structs
{
    [Serializable]
    public struct UnitSpecificationSprite
    {
        public UnitSpecification unitSpecification;
        public Sprite sprite;
    }
}
