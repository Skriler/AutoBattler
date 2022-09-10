using System;
using AutoBattler.Data.Enums;
using UnityEngine;

namespace AutoBattler.Data.Buffs
{
    [Serializable]
    public class SpecificationBuff : BaseBuff
    {
        [SerializeField] private UnitSpecification specification;

        public UnitSpecification Specification => specification;
    }
}
