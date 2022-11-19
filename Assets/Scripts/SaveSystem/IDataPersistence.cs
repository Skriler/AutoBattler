using AutoBattler.SaveSystem.Data;

namespace AutoBattler.SaveSystem
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(GameData data);
    }
}
