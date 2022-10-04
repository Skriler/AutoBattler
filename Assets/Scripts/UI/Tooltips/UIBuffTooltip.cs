using UnityEngine;
using TMPro;
using AutoBattler.Data.Enums;
using AutoBattler.Managers;
using AutoBattler.Data.Buffs;

namespace AutoBattler.UI.Tooltips
{
    public class UIBuffTooltip : UITooltipManager<UIBuffTooltip>
    {
        [Header("Base Characteristics")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textTypeValue;
        [SerializeField] private TextMeshProUGUI textBonus;
        [SerializeField] private TextMeshProUGUI textBonusValue;
        [SerializeField] private TextMeshProUGUI textLevelValue;
        [SerializeField] private TextMeshProUGUI textUnitsAmountValue;

        public override void Setup(Object data)
        {
            if (!(data is Buff))
                return;

            Buff buff = data as Buff;

            SetBaseCharacteristics(buff);
        }

        private void SetBaseCharacteristics(Buff buff)
        {
            textTitle.text = buff.Title;
            textLevelValue.text = buff.CurrentLevel + "/" + buff.MaxLevel;
            textUnitsAmountValue.text = buff.UnitsAmountOnCurrentLevel + "/" + buff.UnitsPerLevel;

            SetType(buff);
            SetDescription(buff);
        }

        private void SetType(Buff buff)
        {
            string strType = "";

            if (buff is RaceBuff)
                strType = (buff as RaceBuff).Race.ToString();
            else if (buff is SpecificationBuff)
                strType = (buff as SpecificationBuff).Specification.ToString();

            textTypeValue.text = strType;
        }

        private void SetDescription(Buff buff)
        {
            string strBonus = buff.TargetCharacteristic switch
            {
                UnitCharacteristic.AttackDamage => "Damage",
                UnitCharacteristic.AttackSpeed => "Speed",
                UnitCharacteristic.Health => "Health",
                _ => "",
            };
            strBonus += " bonus:";

            textBonus.text = strBonus;
            textBonusValue.text = buff.Bonus.ToString();
        }
    }
}
