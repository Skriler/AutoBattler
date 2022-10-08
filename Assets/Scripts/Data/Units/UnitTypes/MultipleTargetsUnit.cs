using System.Collections.Generic;

namespace AutoBattler.Data.Units
{
    public abstract class MultipleTargetsUnit : BaseUnit
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

            foreach (BaseUnit target in currentTargets)
                target.TakeDamage(AttackDamage, DamageType);
        }
    }
}
