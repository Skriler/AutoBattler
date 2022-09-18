using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.Buffs;

namespace AutoBattler.UnitsContainers.Containers
{
    public class FieldContainer : UnitsContainer
    {
        private GameObject unitsContainer;
        private GameObject secondArmyUnitsContainer;
        private FieldGridManager gridManager;
        private BaseUnit[,] units;

        private void OnEnable()
        {
            BuffsEventManager.OnBuffLevelIncreased += AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased += RemoveBuffEffect;
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnBuffLevelIncreased -= AddBuffEffect;
            BuffsEventManager.OnBuffLevelDecreased -= RemoveBuffEffect;
        }

        private void Start()
        {
            unitsContainer = transform.Find("Units").gameObject;
            secondArmyUnitsContainer = transform.Find("SecondArmyUnits").gameObject;
            gridManager = GetComponent<FieldGridManager>();
            units = new BaseUnit[gridManager.ActiveWidth, gridManager.ActiveHeight];
        }

        public BaseUnit[,] GetArmy() => units;

        public override bool IsCellOccupied(Vector2Int index) => units[index.x, index.y] != null;

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            units[index.x, index.y] = unit;
            unit.transform.SetParent(unitsContainer.transform);

            UnitsEventManager.OnUnitAddedOnField(unit);
        }

        public override void RemoveUnit(BaseUnit unit)
        {
            if (!Contains(unit))
                return;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j]?.Id != unit.Id)
                        continue;

                    units[i, j] = null;
                    UnitsEventManager.OnUnitRemovedFromField(unit);
                    return;
                }
            }
        }

        public override void ChangeUnitPosition()
        {

        }

        public override bool Contains(BaseUnit unit)
        {
            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j]?.Id == unit.Id)
                        return true;
                }
            }

            return false;
        }

        public int GetUnitsAmount()
        {
            int unitsAmount = 0;

            for (int i = 0; i < units.GetLength(0); ++i)
            {
                for (int j = 0; j < units.GetLength(1); ++j)
                {
                    if (units[i, j] == null)
                        continue;

                    unitsAmount++;
                }
            }

            return unitsAmount;
        }

        public void AddBuffEffect(Buff buff)
        {
            if (!buff.IsActive())
                return;

            Debug.Log(buff.Title + " added, level: " + buff.CurrentLevel);
        }

        public void RemoveBuffEffect(Buff buff)
        {
            Debug.Log(buff.Title + " removed, level: " + buff.CurrentLevel);
        }

        public void SpawnSecondArmy(BaseUnit[,] army)
        {
            gridManager.SpawnSecondArmy(army, secondArmyUnitsContainer.transform);
        }
    }
}
