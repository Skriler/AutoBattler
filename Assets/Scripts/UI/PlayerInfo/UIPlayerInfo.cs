using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AutoBattler.EventManagers;
using AutoBattler.Managers;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIPlayerInfo : Manager<UIPlayerInfo>
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textTavernTier;
        [SerializeField] private TextMeshProUGUI textGoldenCup;
        [SerializeField] private List<Button> buttons;

        [Header("Solo Mode Components")]
        [SerializeField] private GameObject smallSidePanelBackground;
        [SerializeField] private UIBaseObject goldenCup;

        [Header("Confrontation Mode Components")]
        [SerializeField] private GameObject bigSidePanelBackground;
        [SerializeField] private GameObject membersHealth;

        protected override void Awake()
        {
            base.Awake();

            PlayerEventManager.OnGoldAmountChanged += SetGold;
            PlayerEventManager.OnHealthAmountChanged += SetHealth;
            PlayerEventManager.OnTavernTierIncreased += SetTavernTier;
            PlayerEventManager.OnRoundsWonAmountIncreased += SetGoldenCup;
            FightEventManager.OnFightStarted += DisableButtons;
            FightEventManager.OnFightEnded += EnableButtons;
        }

        protected void OnDestroy()
        {
            PlayerEventManager.OnGoldAmountChanged -= SetGold;
            PlayerEventManager.OnHealthAmountChanged -= SetHealth;
            PlayerEventManager.OnTavernTierIncreased -= SetTavernTier;
            PlayerEventManager.OnRoundsWonAmountIncreased -= SetGoldenCup;
            FightEventManager.OnFightStarted -= DisableButtons;
            FightEventManager.OnFightEnded -= EnableButtons;
        }

        private void SetGold(int gold) => textGold.text = gold.ToString();

        private void SetHealth(int health) => textHealth.text = health.ToString();

        private void SetTavernTier(int tavernTier) => textTavernTier.text = tavernTier.ToString();

        private void SetGoldenCup(int roundsWonAmount) => textGoldenCup.text = roundsWonAmount.ToString();

        private void DisableButtons() => buttons.ForEach(b => b.interactable = false);

        private void EnableButtons() => buttons.ForEach(b => b.interactable = true);

        public void SetActiveSoloModeObjects(bool isActive)
        {
            smallSidePanelBackground.SetActive(isActive);
            goldenCup.gameObject.SetActive(isActive);
        }

        public void SetActiveConfrontationModeObjects(bool isActive)
        {
            bigSidePanelBackground.SetActive(isActive);
            membersHealth.SetActive(isActive);
        }
    }
}
