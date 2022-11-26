using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.Units
{
    public abstract class SingleTargetUnit : FightUnit
    {
        protected BaseUnit currentTarget = null;

        protected override bool HasTarget() => currentTarget != null;

        protected override void DealDamageToTarget()
        {
            if (!HasTarget())
                return;

            currentTarget.TakeDamage(AttackDamage, DamageType);
        }
    }
}
