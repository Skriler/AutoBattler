using UnityEngine;
using TMPro;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Managers;

namespace AutoBattler.UI.Tooltips
{
    public class UIShopUnitTooltip : UITooltipManager<UIShopUnitTooltip>
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textCost;
        [SerializeField] private TextMeshProUGUI textTavernTier;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textAttackDamage;
        [SerializeField] private TextMeshProUGUI textAttackSpeed;

        public override void Setup(Object data)
        {
            if (!(data is UnitCharacteristics))
                return;

            UnitCharacteristics characteristics = data as UnitCharacteristics;

            textTitle.text = characteristics.Title;
            textCost.text = "Cost: " + characteristics.Cost;
            textTavernTier.text = "Tier: " + characteristics.TavernTier;
            textHealth.text = "Health: " + characteristics.MaxHealth;
            textAttackDamage.text = "Damage: " + characteristics.AttackDamage;
            textAttackSpeed.text = "Speed: " + characteristics.AttackSpeed;
        }
    }
}
