using AutoBattler.Data.Units;
using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Buffs;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public class BotFieldContainer : FieldContainer
    {
        protected GridManager fieldGridManager;

        public BuffContainer Buffs { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            fieldGridManager = GetComponent<PlayerFieldGridManager>();
            Buffs = transform.GetComponentInChildren<BuffContainer>();
        }

        public override bool IsCellOccupied(Vector2Int index) => true;

        public override void AddUnit(BaseUnit unit, Vector2Int index)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUnit(BaseUnit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}
