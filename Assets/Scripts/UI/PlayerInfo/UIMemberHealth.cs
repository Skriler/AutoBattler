using UnityEngine;
using UnityEngine.UI;
using AutoBattler.Data.Buffs;
using AutoBattler.EventManagers;
using AutoBattler.UI.Tooltips;
using AutoBattler.Data.Members;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIMemberHealth : UIBaseObject
    {
        [Header("Components")]
        [SerializeField] protected Member member;

        private string startDescription;

        protected override void Awake()
        {
            base.Awake();

            startDescription = Description;
        }

        public void UpdateDescription()
        {
            Description = startDescription + member?.Health;

            UIBaseObjectTooltip.Instance.Setup(this);
        }
    }
}
