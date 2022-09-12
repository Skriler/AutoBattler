using System;
using AutoBattler.Data.Enums;
using AutoBattler.Data.Units;
using UnityEngine;

namespace AutoBattler.Data.Buffs
{
    [Serializable]
    public class RaceBuff : BaseBuff
    {
        [SerializeField] private UnitRace race;

        public UnitRace Race => race;

        public override bool IsUnitPassCheck(BaseUnit unit)
            => unit.Race == Race;
    }
}
