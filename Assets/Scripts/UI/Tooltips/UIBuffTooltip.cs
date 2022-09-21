using UnityEngine;
using TMPro;
using AutoBattler.Data.Units;
using AutoBattler.Managers;
using AutoBattler.Data.Buffs;

namespace AutoBattler.UI.Tooltips
{
    public class UIBuffTooltip : UITooltipManager<UIBuffTooltip>
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textType;
        [SerializeField] private TextMeshProUGUI textCharacteristic;
        [SerializeField] private TextMeshProUGUI textAmount;
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private TextMeshProUGUI textUnitsAmountOnLevel;
        [SerializeField] private TextMeshProUGUI textUnitPerLevel;

        public override void Setup(Object data)
        {
            if (!(data is Buff))
                return;

            Buff buff = data as Buff;

            textTitle.text = buff.Title;
            textType.text = "Type: " + buff.Type.ToString();
            textCharacteristic.text = "Characteristic: " + buff.TargetCharacteristic.ToString();
            textAmount.text = "Amount: " + buff.StatsAmount;
            textLevel.text = "Level: " + buff.CurrentLevel + " / " + buff.MaxLevel;
            textUnitsAmountOnLevel.text = "Units amount on level: " + buff.UnitsAmountOnCurrentLevel;
            textUnitPerLevel.text = "Units per level: " + buff.UnitsPerLevel;
        }
    }
}
