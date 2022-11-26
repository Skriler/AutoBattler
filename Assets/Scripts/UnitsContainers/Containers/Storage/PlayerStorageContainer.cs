using AutoBattler.EventManagers;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.UnitsContainers.Containers.Storage
{
    public class PlayerStorageContainer : MemberStorageContainer
    {
        protected override void Awake()
        {
            UnitsEventManager.OnUnitBought += AddUnit;

            base.Awake();
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitBought -= AddUnit;
        }

        public override void LoadData(GameData data)
        {
            LoadDataFromMemberData(data.player);
        }

        public override void SaveData(GameData data)
        {
            SaveDataToMemberData(data.player);
        }
    }
}
