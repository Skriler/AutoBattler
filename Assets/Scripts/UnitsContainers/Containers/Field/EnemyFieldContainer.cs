using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Buffs;
using AutoBattler.EventManagers;
using AutoBattler.UnitsContainers.Enums;


namespace AutoBattler.UnitsContainers.Containers.Field
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

        public override void AddUnit(BaseUnit unit, Vector2Int index) => units[index.x, index.y] = unit;

        public override void RemoveUnit(BaseUnit unit)
        {
            Vector2Int unitPosition = GetUnitPosition(unit);
            units[unitPosition.x, unitPosition.y] = null;
            unit?.HideHealthBar();
        }

        public void SpawnUnits(BaseUnit[,] units)
        {
            if (units == null)
                return;

            enemyFieldGridManager.SpawnUnits(units, unitsContainer.transform);
            ApplyBuffsForUnits(units);

            this.units = units;
        }

        public void ClearField()
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    enemyFieldGridManager.ChangeTileStatus(new Vector2Int(i, j), TileStatus.Closed);
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
