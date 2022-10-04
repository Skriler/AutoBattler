namespace AutoBattler.Data.Units
{
    public class BringerOfDeath : BaseUnit
    {
        protected BaseUnit currentTarget = null;

        protected override bool HasTargetedEnemy() => currentTarget != null;

        protected override void FindTarget(BaseUnit[,] enemyUnits)
        {
            if (enemyUnits == null)
                return;

            for (int i = enemyUnits.GetLength(0) - 1; i >= 0; --i)
            {
                for (int j = 0; j < enemyUnits.GetLength(1); ++j)
                {
                    if (enemyUnits[i, j] == null)
                        continue;

                    if (!enemyUnits[i, j].IsAlive())
                        continue;

                    currentTarget = enemyUnits[i, j];
                    return;
                }
            }
        }

        protected override void DealDamageToTargetedEnemy()
        {
            if (currentTarget == null)
                return;

            currentTarget.TakeDamage(AttackDamage, DamageType);

            CheckTargetedEnemy();
        }

        protected override void CheckTargetedEnemy()
        {
            if (currentTarget == null)
                return;

            if (!currentTarget.IsAlive())
                currentTarget = null;
        }
    }
}
