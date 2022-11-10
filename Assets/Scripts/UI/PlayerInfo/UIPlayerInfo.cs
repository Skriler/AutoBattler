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
            shopButton.gameObject.SetActive(false);
            startBattleButton.gameObject.SetActive(false);
        }

        private void ShowButtons()
        {
            shopButton.gameObject.SetActive(true);
            startBattleButton.gameObject.SetActive(true);
        }
    }
}
