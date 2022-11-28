using System.Text;
using UnityEngine;

namespace AutoBattler.SaveSystem.Data
{
    public static class PlayerSettings
    {
        public static float MasterVolume { get; set; }
        public static float MusicVolume { get; set; }
        public static float EffectsVolume { get; set; }
        public static float UIVolume { get; set; }
        public static bool IsFullScreen { get; set; }
        public static Resolution Resolution { get; set; }

        public static bool IsMuteOtherFields { get; set; }
        public static int StartHealthAmount { get; set; }
        public static int MaxGoldenCupAmount { get; set; }

        static PlayerSettings()
        {
            MasterVolume = 85;
            MusicVolume = 85;
            EffectsVolume = 85;
            UIVolume = 85;
            IsFullScreen = true;
            Resolution = Screen.currentResolution;

            IsMuteOtherFields = true;
            StartHealthAmount = 0;
            MaxGoldenCupAmount = 0;
        }

        public static string GetSettings()
        {
            StringBuilder settings = new StringBuilder();

            settings
                .Append("MasterVolume: ")
                .AppendLine(MasterVolume.ToString())
                .Append("MusicVolume: ")
                .AppendLine(MusicVolume.ToString())
                .Append("EffectsVolume: ")
                .AppendLine(EffectsVolume.ToString())
                .Append("UIVolume: ")
                .AppendLine(UIVolume.ToString())
                .Append("IsFullScreen: ")
                .AppendLine(IsFullScreen.ToString())
                .Append("Resolution: ")
                .AppendLine(Resolution.ToString())
                .Append("IsMuteOtherFields: ")
                .AppendLine(IsMuteOtherFields.ToString())
                .Append("StartHealthAmount: ")
                .AppendLine(StartHealthAmount.ToString())
                .Append("MaxGoldenCupAmount: ")
                .AppendLine(MaxGoldenCupAmount.ToString());

            return settings.ToString();
        }
    }
}
