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

        private void Awake()
        {
            PlayerEventManager.OnGoldAmountChanged += SetGold;
            PlayerEventManager.OnHealthAmountChanged += SetHealth;
            PlayerEventManager.OnTavernTierChanged += SetTavernTier;
            FightEventManager.OnFightStarted += DisableButtons;
            FightEventManager.OnFightEnded += EnableButtons;
        }

        private void OnDestroy()
        {
            PlayerEventManager.OnGoldAmountChanged -= SetGold;
            PlayerEventManager.OnHealthAmountChanged -= SetHealth;
            PlayerEventManager.OnTavernTierChanged -= SetTavernTier;
            FightEventManager.OnFightStarted -= DisableButtons;
            FightEventManager.OnFightEnded -= EnableButtons;
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

        private void DisableButtons()
        {
            shopButton.gameObject.SetActive(false);
            startBattleButton.gameObject.SetActive(false);
        }

        private void EnableButtons()
        {
            shopButton.gameObject.SetActive(true);
            startBattleButton.gameObject.SetActive(true);
        }
    }
}
