using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;
using AutoBattler.Managers;

namespace AutoBattler.UI.Tooltips
{
    public class UIUnitTooltip : UITooltipManager<UIUnitTooltip>
    {
        [Header("Base Characteristics")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textHealthValue;
        [SerializeField] private TextMeshProUGUI textAttackDamageValue;
        [SerializeField] private TextMeshProUGUI textAttackSpeedValue;

        [Header("Images")]
        [SerializeField] private Image imageUnitRace;
        [SerializeField] private Image imageUnitSpecification;
        [SerializeField] private Image imageDamageType;

        [Header("Protection Values")]
        [SerializeField] private TextMeshProUGUI textFireProtectionValue;
        [SerializeField] private TextMeshProUGUI textIceProtectionValue;
        [SerializeField] private TextMeshProUGUI textChaosProtectionValue;
        [SerializeField] private TextMeshProUGUI textPurifyProtectionValue;

        public BaseUnit CurrentUnit { get; private set; }

        public override void Setup(Object data)
        {
            if (!(data is BaseUnit))
                return;

            CurrentUnit = data as BaseUnit;

            SetBaseCharacteristics(CurrentUnit);
            SetImageSprites(CurrentUnit);
            SetDamageTypesProtectionValues(CurrentUnit);
        }

        private void SetBaseCharacteristics(BaseUnit unit)
        {
            textTitle.text = CurrentUnit.Title;
            textHealthValue.text = CurrentUnit.Health + "/" + CurrentUnit.MaxHealth;
            textAttackDamageValue.text = CurrentUnit.AttackDamage.ToString();
            textAttackSpeedValue.text = CurrentUnit.AttackSpeed.ToString();
        }

        private void SetImageSprites(BaseUnit unit)
        {
            imageUnitRace.sprite = ImageManager.Instance.GetUnitRaceSprite(unit.Race);
            imageUnitSpecification.sprite = ImageManager.Instance.GetUnitSpecificationSprite(unit.Specification);
            imageDamageType.sprite = ImageManager.Instance.GetDamageTypeSprite(unit.DamageType);
        }

        private void SetDamageTypesProtectionValues(BaseUnit unit)
        {
            textFireProtectionValue.text = unit.GetDamageTypeProtection(DamageType.Fire).ToString() + "%";
            textIceProtectionValue.text = unit.GetDamageTypeProtection(DamageType.Ice).ToString() + "%";
            textChaosProtectionValue.text = unit.GetDamageTypeProtection(DamageType.Chaos).ToString() + "%";
            textPurifyProtectionValue.text = unit.GetDamageTypeProtection(DamageType.Purify).ToString() + "%";
        }
    }
}
