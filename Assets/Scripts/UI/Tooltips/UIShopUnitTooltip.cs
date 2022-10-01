using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Managers;
using AutoBattler.Data.Enums;

namespace AutoBattler.UI.Tooltips
{
    public class UIShopUnitTooltip : UITooltipManager<UIShopUnitTooltip>
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textCostValue;
        [SerializeField] private TextMeshProUGUI textTavernTierValue;
        [SerializeField] private TextMeshProUGUI textHealthValue;
        [SerializeField] private TextMeshProUGUI textAttackDamageValue;
        [SerializeField] private TextMeshProUGUI textAttackSpeedValue;

        [SerializeField] private Image ImageUnitRace;
        [SerializeField] private Image ImageUnitSpecification;
        [SerializeField] private Image imageDamageType;

        [SerializeField] private TextMeshProUGUI textFireProtectionValue;
        [SerializeField] private TextMeshProUGUI textIceProtectionValue;
        [SerializeField] private TextMeshProUGUI textChaosProtectionValue;
        [SerializeField] private TextMeshProUGUI textPurifyProtectionValue;

        public override void Setup(Object data)
        {
            if (!(data is UnitCharacteristics))
                return;

            UnitCharacteristics characteristics = data as UnitCharacteristics;

            textTitle.text = characteristics.Title;
            textCostValue.text = characteristics.Cost.ToString();
            textTavernTierValue.text = characteristics.TavernTier.ToString();
            textHealthValue.text = characteristics.MaxHealth.ToString();
            textAttackDamageValue.text = characteristics.AttackDamage.ToString();
            textAttackSpeedValue.text = characteristics.AttackSpeed.ToString();

            

            SetDamageTypesProtectionValues(characteristics);
        }

        private void SetDamageTypesProtectionValues(UnitCharacteristics characteristics)
        {
            DamageTypeProtection[] damageTypesProtection = characteristics.DamageTypesProtectionPercentage;

            foreach (DamageTypeProtection damageTypeProtection in damageTypesProtection)
            {
                switch (damageTypeProtection.damageType)
                {
                    case DamageType.Fire:
                        textFireProtectionValue.text = damageTypeProtection.protectionPercentage + " %";
                        break;
                    case DamageType.Ice:
                        textIceProtectionValue.text = damageTypeProtection.protectionPercentage + " %";
                        break;
                    case DamageType.Chaos:
                        textChaosProtectionValue.text = damageTypeProtection.protectionPercentage + " %";
                        break;
                    case DamageType.Purify:
                        textPurifyProtectionValue.text = damageTypeProtection.protectionPercentage + " %";
                        break;
                }
            }
        }
    }
}
