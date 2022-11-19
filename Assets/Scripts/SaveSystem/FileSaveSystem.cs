using System;
using System.IO;
using UnityEngine;
using AutoBattler.Data.Player;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.SaveSystem
{
    public static class FileSaveSystem
    {
        private static string PROGRESS_PATH = Application.persistentDataPath + "/playerProgress.data";
        private static string SETTINGS_PATH = Application.persistentDataPath + "/playerSettings.data";

        public static bool IsSavedProgress() => File.Exists(PROGRESS_PATH);

        public static void DeleteSavedProgress() => File.Delete(PROGRESS_PATH);

        public static void SaveProgress(GameData data, bool useEncryption, string encryptionCodeWord)
        {
            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
                dataToStore = EncryptDecrypt(dataToStore, encryptionCodeWord);

            using (FileStream stream = new FileStream(PROGRESS_PATH, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

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

        public static GameData LoadProgress(bool useEncryption, string encryptionCodeWord)
        {
            GameData loadedData = null;

            if (!File.Exists(PROGRESS_PATH))
                return loadedData;

            try
            {
                string dataToLoad;

                using (FileStream stream = new FileStream(PROGRESS_PATH, FileMode.Open))
                {
                    using (TextReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad, encryptionCodeWord);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error while reading progress from file \n" + e);
            }

            return loadedData;
        }

        public static void LoadSettings()
        {
            if (!File.Exists(SETTINGS_PATH))
                return;

            try
            {
                using (FileStream stream = new FileStream(SETTINGS_PATH, FileMode.Open))
                {
                    using (TextReader reader = new StreamReader(stream))
                    {
                        string str;
                        while ((str = reader.ReadLine()) != null)
                        {
                            string[] setting = str.Split(':');
                            SetSetting(setting[0], setting[1]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while reading settings from file \n" + e);
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

        private static string EncryptDecrypt(string data, string encryptionCodeWord)
        {
            string modifiedData = "";

            for (int i = 0; i < data.Length; ++i)
                modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);

            return modifiedData;
        }
    }
}
