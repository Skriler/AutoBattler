namespace AutoBattler.Data.Units
{
    public abstract class SingleTargetUnit : BaseUnit
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
