using AutoBattler.EventManagers;

namespace AutoBattler.UnitsContainers.Grids
{
    public class PlayerFieldGridManager : MemberFieldGridManager
    {
        protected override void Awake()
        {
            base.Awake();

            PlayerEventManager.OnTavernTierIncreased += OpenTiles;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PlayerEventManager.OnTavernTierIncreased -= OpenTiles;
        }
    }
}
