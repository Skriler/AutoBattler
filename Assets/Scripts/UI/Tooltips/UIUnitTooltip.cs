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

        public override void Setup(Object data)
        {
            if (!(data is BaseUnit))
                return;

            BaseUnit unit = data as BaseUnit;

            textTitle.text = unit.Title;
            textHealth.text = "Health: " + unit.Health + " / " + unit.MaxHealth;
            textAttackDamage.text = "Damage: " + unit.AttackDamage;
            textAttackSpeed.text = "Speed: " + unit.AttackSpeed;
        }
    }
}
