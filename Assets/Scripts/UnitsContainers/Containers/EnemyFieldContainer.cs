using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Buffs;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Containers
{
    public class EnemyFieldContainer : FieldContainer
    {
        protected EnemyFieldGridManager enemyFieldGridManager;

        public BuffContainer Buffs { get; private set; }

        protected override void Awake()
        {
            FightEventManager.OnFightEnded += ClearField;

            base.Awake();

            enemyFieldGridManager = GetComponent<EnemyFieldGridManager>();
            Buffs = transform.GetComponentInChildren<BuffContainer>();
        }

        protected void OnDestroy()
        {
            FightEventManager.OnFightEnded -= ClearField;
        }

        public override bool IsCellOccupied(Vector2Int index) => true;

        public void SpawnUnits()
        {
            if (units == null)
                return;

            enemyFieldGridManager.SpawnUnits(units, unitsContainer.transform);
            ApplyBuffsForUnits(units);
        }

        public void ClearField()
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    Destroy(units[i, j].gameObject);
                    units[i, j] = null;
                }
            }

            Buffs.ResetBuffs();
        }

        private void ApplyBuffsForUnits(BaseUnit[,] units)
        {
            foreach (BaseUnit unit in units)
            {
                if (unit == null)
                    continue;

                Buffs.AddUnitBuffs(unit);
            }

            foreach (BaseUnit unit in units)
            {
                if (unit == null)
                    continue;

                Buffs.ApplyBuffsForUnit(unit);
            }
        }
    }
}
