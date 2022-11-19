using UnityEngine;
using UnityEngine.UI;
using AutoBattler.Data.Buffs;
using AutoBattler.EventManagers;
using AutoBattler.UI.Tooltips;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIBuffWrapper : MonoBehaviour
    {
        private static float MAX_COLOR_VALUE = 255;

        [Header("Components")]
        [SerializeField] private Image buffImage;

        [Header("Parameters")]
        [SerializeField] private Buff buff;
        [SerializeField] private float minColorValue = 75;

        private float minColor—oefficient;

        private void Awake()
        {
            BuffsEventManager.OnBuffLevelIncreased += ChangeBuffImageColor;
            BuffsEventManager.OnBuffLevelDecreased += ChangeBuffImageColor;
            SaveSystemEventManager.OnDataLoaded += ChangeBuffImageColor;
        }

        private void OnDestroy()
        {
            BuffsEventManager.OnBuffLevelIncreased -= ChangeBuffImageColor;
            BuffsEventManager.OnBuffLevelDecreased -= ChangeBuffImageColor;
            SaveSystemEventManager.OnDataLoaded -= ChangeBuffImageColor;
        }

        private void Start()
        {
            minColor—oefficient = minColorValue / MAX_COLOR_VALUE;
            ChangeBuffImageColor(buff);
        }

        public void MouseExit() => UIBuffTooltip.Instance.Hide();

        public void MouseEnter()
        {
            UIBuffTooltip.Instance.Show();
            UIBuffTooltip.Instance.Setup(buff);
        }

        public void ChangeBuffImageColor(Buff buff)
        {
            if (this.buff != buff)
                return;

            ChangeBuffImageColor();
        }

        public void ChangeBuffImageColor()
        {
            float level—oefficient = (float)buff.CurrentLevel / buff.MaxLevel;

            float color—oefficient = level—oefficient * (1 - minColor—oefficient);
            color—oefficient += minColor—oefficient;

            buffImage.color = new Color(color—oefficient, color—oefficient, color—oefficient);
        }
    }
}
