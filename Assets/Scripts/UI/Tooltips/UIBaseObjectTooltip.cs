using UnityEngine;
using TMPro;
using AutoBattler.Managers;
using AutoBattler.Data.ScriptableObjects.Characteristics;

namespace AutoBattler.UI.Tooltips
{
    public class UIBaseObjectTooltip : UITooltipManager<UIBaseObjectTooltip>
    {
        [Header("Base Characteristics")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textDescription;

        public override void Setup(Object data)
        {
            if (!(data is UIBaseObjectCharacteristics))
                return;

            UIBaseObjectCharacteristics characteristics = data as UIBaseObjectCharacteristics;

            textTitle.text = characteristics.Title;
            textDescription.text = characteristics.Description; 
        }
    }
}
