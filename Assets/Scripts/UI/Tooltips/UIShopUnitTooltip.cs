using UnityEngine;
using TMPro;
using AutoBattler.Data.ScriptableObjects.Characteristics;

namespace AutoBattler.UI.Tooltips
{
    public class UIShopUnitTooltip : UITooltip
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textCost;
        [SerializeField] private TextMeshProUGUI textTavernTier;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textAttackDamage;
        [SerializeField] private TextMeshProUGUI textAttackSpeed;

        public override void Setup(ScriptableObject data)
        {
            if (!(data is UnitCharacteristics))
                return;

            UnitCharacteristics characteristics = data as UnitCharacteristics;

            textTitle.text = characteristics.Title;
            textCost.text = "Cost: " + characteristics.Cost.ToString();
            textTavernTier.text = "Tier: " + characteristics.TavernTier.ToString();
            textHealth.text = "Health: " + characteristics.MaxHealth.ToString();
            textAttackDamage.text = "Damage: " + characteristics.AttackDamage.ToString();
            textAttackSpeed.text = "Speed: " + characteristics.AttackSpeed.ToString();
        }
    }
}
