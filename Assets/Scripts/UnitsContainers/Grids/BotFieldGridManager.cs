using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Members;

namespace AutoBattler.UnitsContainers.Grids
{
    public class BotFieldGridManager : MemberFieldGridManager
    {
        [Header("Components")]
        [SerializeField] protected Bot owner;

        protected override void Awake()
        {
            base.Awake();

            BotsEventManager.OnTavernTierIncreased += OpenTiles;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            BotsEventManager.OnTavernTierIncreased -= OpenTiles;
        }

        protected void OpenTiles(int tavernTier, string id)
        {
            if (owner.Id != id)
                return;

            OpenTiles(tavernTier);
        }
    }
}
