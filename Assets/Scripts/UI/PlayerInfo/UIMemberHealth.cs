using UnityEngine;
using UnityEngine.UI;
using AutoBattler.Data.Buffs;
using AutoBattler.EventManagers;
using AutoBattler.UI.Tooltips;
using AutoBattler.Data.Members;
using AutoBattler.Managers;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIMemberHealth : UIBaseObject
    {
        [Header("Components")]
        [SerializeField] protected Member owner;

        private string startDescription;

        protected override void Awake()
        {
            base.Awake();

            startDescription = Description;
        }

        public void UpdateDescription()
        {
            Description = startDescription + owner?.Health;

            UIBaseObjectTooltip.Instance.Setup(this);
        }
    }
}
