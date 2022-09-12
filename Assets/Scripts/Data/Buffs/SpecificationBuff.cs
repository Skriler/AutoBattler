using System;
using AutoBattler.Data.Enums;
using AutoBattler.Data.Units;
using UnityEngine;

namespace AutoBattler.Data.Buffs
{
    [Serializable]
    public class SpecificationBuff : BaseBuff
    {
        [SerializeField] private UnitSpecification specification;

        public UnitSpecification Specification => specification;

        public override bool IsUnitPassCheck(BaseUnit unit)
            => unit.Specification == Specification;
    }
}
