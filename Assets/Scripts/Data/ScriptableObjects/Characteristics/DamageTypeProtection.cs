using System;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [Serializable]
    public struct DamageTypeProtection
    {
        public DamageType damageType;
        public int protectionPercentage;
    }
}
