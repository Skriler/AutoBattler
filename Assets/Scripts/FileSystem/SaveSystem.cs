using System;
using System.IO;
using UnityEngine;
using AutoBattler.Data.Player;

namespace AutoBattler.FileSystem
{
    public static class SaveSystem
    {
        private static string SETTINGS_PATH = Application.persistentDataPath + "/playerSettings.data";

        public static void SaveSettings()
        {
            using (FileStream stream = new FileStream(SETTINGS_PATH, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.Write(PlayerSettings.GetSettings());
                }
            }
        }

        public static void LoadSettings()
        {
            if (!File.Exists(SETTINGS_PATH))
                return;

            using (FileStream stream = new FileStream(SETTINGS_PATH, FileMode.Open))
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    string str;
                    while ((str = reader.ReadLine()) != null)
                    {
                        string[] setting = str.Split(':');
                        try
                        {
                            SetSetting(setting[0], setting[1]);
                        }
                        catch
                        {
                            Debug.LogError("Error while reading a data from a file");
                        }
                    }
                }
            }
        }

        private static void SetSetting(string settingName, string value)
        {
            switch (settingName)
            {
                case "MasterVolume":
                    PlayerSettings.MasterVolume = float.Parse(value);
                    break;
                case "MusicVolume":
                    PlayerSettings.MusicVolume = float.Parse(value);
                    break;
                case "EffectsVolume":
                    PlayerSettings.EffectsVolume = float.Parse(value);
                    break;
                case "UIVolume":
                    PlayerSettings.UIVolume = float.Parse(value);
                    break;
                case "IsFullScreen":
                    PlayerSettings.IsFullScreen = bool.Parse(value);
                    break;
                case "Resolution":
                    PlayerSettings.Resolution = ParseResolution(value);
                    break;
            }
        }

        private static Resolution ParseResolution(string value)
        {
            Resolution resolution = new Resolution();
            string[] resolutionValues = value.Split('x', '@', 'H');
            
            resolution.width = int.Parse(resolutionValues[0]);
            resolution.height = int.Parse(resolutionValues[1]);
            resolution.refreshRate = int.Parse(resolutionValues[2]);

            return resolution;
        }
    }
}
