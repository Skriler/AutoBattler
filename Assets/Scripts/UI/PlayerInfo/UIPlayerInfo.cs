using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AutoBattler.EventManagers;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIPlayerInfo : MonoBehaviour
    {
        [Header("Text Characteristics")]
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textTavernTier;

        [Header("Buttons")]
        [SerializeField] private Button shopButton;
        [SerializeField] private Button startBattleButton;
        [SerializeField] private Button manualButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            PlayerEventManager.OnGoldAmountChanged += SetGold;
            PlayerEventManager.OnHealthAmountChanged += SetHealth;
            PlayerEventManager.OnTavernTierIncreased += SetTavernTier;
            FightEventManager.OnFightStarted += HideButtons;
            FightEventManager.OnFightEnded += ShowButtons;
        }

        private void OnDestroy()
        {
            PlayerEventManager.OnGoldAmountChanged -= SetGold;
            PlayerEventManager.OnHealthAmountChanged -= SetHealth;
            PlayerEventManager.OnTavernTierIncreased -= SetTavernTier;
            FightEventManager.OnFightStarted -= HideButtons;
            FightEventManager.OnFightEnded -= ShowButtons;
        }

        private void SetGold(int gold)
        {
            textGold.text = gold.ToString();
        }

        private void SetHealth(int health)
        {
            textHealth.text = health.ToString();
        }

        private void SetTavernTier(int tavernTier)
        {
            textTavernTier.text = tavernTier.ToString();
        }

        private void HideButtons()
        {
            shopButton.interactable = false;
            startBattleButton.interactable = false;
            manualButton.interactable = false;
            exitButton.interactable = false;
        }

        private void ShowButtons()
        {
            shopButton.interactable = true;
            startBattleButton.interactable = true;
            manualButton.interactable = true;
            exitButton.interactable = true;
        }
    }
}
