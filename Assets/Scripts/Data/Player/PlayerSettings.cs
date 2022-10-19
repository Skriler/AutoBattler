using System.Text;
using UnityEngine;

namespace AutoBattler.Data.Player
{
    public static class PlayerSettings
    {
        public static float MasterVolume { get; set; }
        public static float MusicVolume { get; set; }
        public static float EffectsVolume { get; set; }
        public static float UIVolume { get; set; }
        public static bool IsFullScreen { get; set; }
        public static Resolution Resolution { get; set; }

        static PlayerSettings()
        {
            MasterVolume = 85;
            MusicVolume = 85;
            EffectsVolume = 85;
            UIVolume = 85;
            IsFullScreen = true;
            Resolution = Screen.currentResolution;
        }

        public static string GetSettings()
        {
            StringBuilder settings = new StringBuilder();

            settings.Append("MasterVolume: ");
            settings.Append(MasterVolume);
            settings.Append("\nMusicVolume: ");
            settings.Append(MusicVolume);
            settings.Append("\nEffectsVolume: ");
            settings.Append(EffectsVolume);
            settings.Append("\nUIVolume: ");
            settings.Append(UIVolume);
            settings.Append("\nIsFullScreen: ");
            settings.Append(IsFullScreen);
            settings.Append("\nResolution: ");
            settings.Append(Resolution);

            return settings.ToString();
        }
    }
}
