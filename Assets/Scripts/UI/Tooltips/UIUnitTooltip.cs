using UnityEngine;
using TMPro;
using AutoBattler.Data.Units;
using AutoBattler.Managers;

namespace AutoBattler.UI.Tooltips
{
    public class UIUnitTooltip : UITooltipManager<UIUnitTooltip>
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textAttackDamage;
        [SerializeField] private TextMeshProUGUI textAttackSpeed;

        public BaseUnit CurrentUnit { get; private set; }

        public override void Setup(Object data)
        {
            if (!(data is BaseUnit))
                return;

            CurrentUnit = data as BaseUnit;

            textTitle.text = CurrentUnit.Title;
            textHealth.text = CurrentUnit.Health + " / " + CurrentUnit.MaxHealth;
            textAttackDamage.text = CurrentUnit.AttackDamage.ToString();
            textAttackSpeed.text = CurrentUnit.AttackSpeed.ToString();
        }
    }
}
