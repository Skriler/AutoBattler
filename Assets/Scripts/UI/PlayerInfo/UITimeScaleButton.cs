using UnityEngine;
using AutoBattler.UI.Tooltips;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using Unity.VisualScripting;

namespace AutoBattler.UI.PlayerInfo
{
    public class UITimeScaleButton : UIBaseObject
    {

        private string startDescription;

        protected override void Awake()
        {
            base.Awake();

            startDescription = Description;
        }

        public override void MouseEnter()
        {
            UpdateDescription();
            base.MouseEnter();
        }

        public void UpdateDescription()
        {
            Description = startDescription + Time.timeScale + "x";
            UIBaseObjectTooltip.Instance.Setup(this);
        }
    }
}
