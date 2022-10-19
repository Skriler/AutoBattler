using System.IO;
using UnityEngine;
using AutoBattler.Data.Player;

namespace AutoBattler.FileSystem
{
    public static class SaveSystem
    {
        private static string settingsPath = Application.persistentDataPath + "/playerSettings.data";

        public static void SaveSettings()
        {
            using (FileStream stream = new FileStream(settingsPath, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.Write(PlayerSettings.GetSettings());
                }
            }
        }
    }
}
