using UnityEngine;
using TMPro;
using AutoBattler.Managers;
using AutoBattler.UI.PlayerInfo;

namespace AutoBattler.UI.Tooltips
{
    public class UIBaseObjectTooltip : UITooltipManager<UIBaseObjectTooltip>
    {
        [Header("Base Characteristics")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textDescription;

        public override void Setup(Object data)
        {
            if (!(data is UIBaseObject))
                return;

            UIBaseObject baseObject = data as UIBaseObject;

            textTitle.text = baseObject.Title;
            textDescription.text = baseObject.Description; 
        }
    }
}
