using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler.Data.Units
{
    public abstract class MultipleTargetsUnit : FightUnit
    {
        protected List<BaseUnit> currentTargets;

        protected override void Start()
        {
            currentTargets = new List<BaseUnit>();
        }

        protected override bool HasTarget() => currentTargets.Count != 0;

        protected override void DealDamageToTarget()
        {
            if (!HasTarget())
                return;

            float damage;

            foreach (BaseUnit target in currentTargets)
            {
                damage = Random.value > criticalHitChance ? 
                    AttackDamage : 
                    AttackDamage * criticalHitAttackMultiplier;

                target.TakeDamage(
                    AttackDamage, 
                    DamageType,
                    damage > AttackDamage
                    );
            }
        }
    }
}
