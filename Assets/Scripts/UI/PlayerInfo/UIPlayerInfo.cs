using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AutoBattler.EventManagers;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIPlayerInfo : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textTavernTier;
        [SerializeField] private TextMeshProUGUI textGoldenCup;
        [SerializeField] private List<Button> buttons;

        private void Awake()
        {
            PlayerEventManager.OnGoldAmountChanged += SetGold;
            PlayerEventManager.OnHealthAmountChanged += SetHealth;
            PlayerEventManager.OnTavernTierIncreased += SetTavernTier;
            PlayerEventManager.OnRoundsWonAmountIncreased += SetGoldenCup;
            FightEventManager.OnFightStarted += DisableButtons;
            FightEventManager.OnFightEnded += EnableButtons;
        }

        private void OnDestroy()
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
    }
}
