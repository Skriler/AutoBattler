using UnityEngine;

namespace AutoBattler.UnitsContainers.Containers.Storage
{ 
    public class BotStorageContainer : StorageContainer
    {
        public int GetUnitsAmount()
        {
            int unitsAmount = 0;

            for (int i = 0; i < units.Length; ++i)
            {
                if (units[i] == null)
                    continue;

                ++unitsAmount;
            }

            return unitsAmount;
        }
    }
}
