using UnityEngine;

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

            float damage = Random.value > criticalHitChance ? 
                AttackDamage : 
                AttackDamage * criticalHitAttackMultiplier;

            currentTarget.TakeDamage(
                damage, 
                DamageType, 
                damage > AttackDamage
                );
        }
    }
}
