using System.Collections.Generic;
using UnityEngine;
using AutoBattler.UnitsContainers.Grids;
using AutoBattler.Data.Buffs;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.UnitsContainers.Containers.Field
{
    public abstract class MemberFieldContainer : FieldContainer
    {
        protected MemberFieldGridManager memberFieldGridManager;

        public BuffContainer Buffs { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            memberFieldGridManager = GetComponent<MemberFieldGridManager>();
            Buffs = transform.GetComponentInChildren<BuffContainer>();
        }

        public override bool IsCellOccupied(Vector2Int index) => units[index.x, index.y] != null;

        public int GetOpenedCellsAmount() => memberFieldGridManager.GetOpenedCellsAmount();

        public List<TavernTierOpenedTiles> GetTavernTierOpenedTiles(int tavernTier) =>
            memberFieldGridManager.GetTavernTierOpenedTiles(tavernTier);
    }
}
