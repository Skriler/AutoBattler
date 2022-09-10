using System;
using AutoBattler.Data.Enums;
using UnityEngine;

namespace AutoBattler.Data.Buffs
{
    [Serializable]
    public class RaceBuff : BaseBuff
    {
        [SerializeField] private UnitRace race;

        public UnitRace Race => race;
    }
}
