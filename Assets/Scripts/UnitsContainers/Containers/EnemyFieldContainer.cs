using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Containers
{
    public class EnemyFieldContainer : FieldContainer
    {
        protected EnemyFieldGridManager enemyFieldGridManager;

        protected void Awake()
        {
            FightEventManager.OnFightEnded += ClearField;
        }

        protected void OnDestroy()
        {
            FightEventManager.OnFightEnded -= ClearField;
        }

        protected override void Start()
        {
            base.Start();

            enemyFieldGridManager = GetComponent<EnemyFieldGridManager>();
        }

        public override bool IsCellOccupied(Vector2Int index) => true;

        public void SpawnUnits(BaseUnit[,] army)
        {
            enemyFieldGridManager.SpawnUnits(army, unitsContainer.transform);
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
        }
    }
}
