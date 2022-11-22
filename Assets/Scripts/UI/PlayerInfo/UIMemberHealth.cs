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

            PlayerEventManager.OnHealthAmountChanged += CheckPlayersHealth;
            BotsEventManager.OnHealthAmountChanged += CheckOwnersHealth;

            startDescription = Description;
        }

        protected void OnDestroy()
        {
            PlayerEventManager.OnHealthAmountChanged -= CheckPlayersHealth;
            BotsEventManager.OnHealthAmountChanged -= CheckOwnersHealth;
        }

        private void CheckPlayersHealth(int health)
        {
            if (owner is Player)
                UpdateDescription();
        }

        private void CheckOwnersHealth(int health, string id)
        {
            if (owner.Id != id)
                return;

            UpdateDescription();
        }

        public void UpdateDescription()
        {
            Description = startDescription + owner?.Health;

            UIBaseObjectTooltip.Instance.Setup(this);
        }
    }
}
